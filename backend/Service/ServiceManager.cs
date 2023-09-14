using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;

namespace Service;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IHabitService> _habitService;
    private readonly Lazy<IRepetitionService> _repetitionService;
    private readonly Lazy<ISeedService> _seedService;
    private readonly Lazy<IAuthenticationService> _authenticationService;

    public ServiceManager(IRepositoryManager repositoryManager,
                          ILoggerManager logger,
                          IMapper mapper,
                          UserManager<User> userManager,
                          IConfiguration configuration)
    {
        _habitService = new Lazy<IHabitService>(() => 
            new HabitService(repositoryManager, logger, mapper));
        _repetitionService = new Lazy<IRepetitionService>(() => 
            new RepetitionService(repositoryManager, logger, mapper));
        _seedService = new Lazy<ISeedService>(() => 
            new SeedService(repositoryManager, logger, mapper));
        _authenticationService = new Lazy<IAuthenticationService>(() => 
            new AuthenticationService(logger, mapper, userManager, configuration));
    }

    public IHabitService HabitService => _habitService.Value;
    public IRepetitionService RepetitionService => _repetitionService.Value;
    public ISeedService SeedService => _seedService.Value;
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}
