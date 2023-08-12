using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace LoopHabits.Presentation.Controllers;

[Route("api/habits")]
[ApiController]
public class HabitsController : ControllerBase
{
    private readonly IServiceManager _service;

    public HabitsController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetHabits()
    {
        var habits = await _service.HabitService.GetAllHabitsAsync(trackChanges: false);
        return Ok(habits);
    }

    [HttpGet("{id:guid}", Name = "HabitById")]
    public async Task<IActionResult> GetHabit(Guid id)
    {
        var habit = await _service.HabitService.GetHabitAsync(id, trackChanges: false);
        return Ok(habit);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateHabit([FromBody] HabitForCreationDto habit)
    {
        var createdHabit = await _service.HabitService.CreateHabitAsync(habit);
        return CreatedAtRoute("HabitById", new { id = createdHabit.Id }, createdHabit);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteHabit(Guid id)
    {
        await _service.HabitService.DeleteHabitAsync(id, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateHabit(Guid id, [FromBody] HabitForUpdateDto habit)
    {
        await _service.HabitService.UpdateHabitAsync(id, habit, trackChanges: true);
        return NoContent();
    }
}
