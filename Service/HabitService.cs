using Contracts;
using Service.Contracts;

namespace Service;

internal sealed class HabitService : IHabitService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public HabitService(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
