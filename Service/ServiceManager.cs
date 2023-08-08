using AutoMapper;
using Contracts;
using Service.Contracts;

namespace Service;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IHabitService> _habitService;
    private readonly Lazy<IRepetitionService> _repetitionService;
    private readonly Lazy<ISeedService> _seedService;

    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
    {
        _habitService = new Lazy<IHabitService>(() => new HabitService(repositoryManager, logger, mapper));
        _repetitionService = new Lazy<IRepetitionService>(() => new RepetitionService(repositoryManager, logger, mapper));
        _seedService = new Lazy<ISeedService>(() => new SeedService(repositoryManager, logger));
    }

    public IHabitService HabitService => _habitService.Value;

    public IRepetitionService RepetitionService => _repetitionService.Value;

    public ISeedService SeedService => _seedService.Value;
}
