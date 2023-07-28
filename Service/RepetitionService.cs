using Contracts;
using Service.Contracts;

namespace Service;

internal sealed class RepetitionService : IRepetitionService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public RepetitionService(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
