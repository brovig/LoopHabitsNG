using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace LoopHabits.Presentation.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class SeedController : ControllerBase
{
    private readonly IServiceManager _service;

    public SeedController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Import()
    {
        var addedHabbits = new List<HabitDto>();
        var addedRepetitions = new List<RepetitionDto>();

        // read all habits and repetitions from sqlite db
        var habitsFromBackup = await _service.SeedService.GetAllHabitsFromBackupAsync();
        if (!habitsFromBackup.Any())
            return Ok("No habits found in backup db file");

        var repetitionsFromBackup = await _service.SeedService.GetAllRepetitionsFromBackupAsync();

        // create a lookup dictionaries containing all the habits already existing in postgres db
        var habitsFromDb = await _service.HabitService.GetAllHabitsAsync(trackChanges: false);
        var habitsByName = new Dictionary<string, HabitDto>();
        foreach (var habit in habitsFromDb)
        {
            habitsByName[habit.Name] = habit;
        }

        var habitsToAdd = habitsFromBackup.Where(h => !habitsByName.ContainsKey(h.Name!)).ToList();


        // adding habits and repetitions to postgres db
        foreach (var habitFromBackup in habitsToAdd)
        {
            var habitDtoToAdd = _service.SeedService.MapHabitForCreation(habitFromBackup);
            var repetitionsToAdd = _service.SeedService.MapRepetitionsForCreation(repetitionsFromBackup.Where(r => r.Habit == habitFromBackup.Id));

            var addedHabit = await _service.HabitService.CreateHabitAsync(habitDtoToAdd);
            var addedRepetitionCollection = await _service.RepetitionService.CreateRepetitionCollectionAsync(addedHabit.Id, repetitionsToAdd, trackChanges: false);

            addedHabbits.Add(addedHabit);
            foreach (var addedRep in addedRepetitionCollection)
                addedRepetitions.Add(addedRep);
        }

        return new JsonResult(new
        {
            Habits = addedHabbits,
            Repetitions = addedRepetitions
        });
    }
}
