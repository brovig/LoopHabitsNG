namespace Service.Contracts;

public interface IServiceManager
{
    IHabitService HabitService { get; }
    IRepetitionService RepetitionService { get; }
    ISeedService SeedService { get; }
}
