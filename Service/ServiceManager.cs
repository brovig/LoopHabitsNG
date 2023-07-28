using Contracts;
using Service.Contracts;

namespace Service;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IHabitService> _habitService;
    private readonly Lazy<IRepetitionService> _repetitionService;

    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger)
    {
        _habitService = new Lazy<IHabitService>(() => new HabitService(repositoryManager, logger));
        _repetitionService = new Lazy<IRepetitionService>(() => new RepetitionService(repositoryManager, logger));
    }

    public IHabitService HabitService => _habitService.Value;

    public IRepetitionService RepetitionService => _repetitionService.Value;
}
