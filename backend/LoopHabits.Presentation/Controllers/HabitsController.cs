using LoopHabits.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using System.Net;

namespace LoopHabits.Presentation.Controllers;

[Route("api/habits")]
[ApiController]
[Authorize]
public class HabitsController : ControllerBase
{
    private readonly IServiceManager _service;

    public HabitsController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetHabits([FromHeader] string authorization)
    {
        var userId = await _service.AuthenticationService.GetUserId(authorization);

        var habits = await _service.HabitService.GetAllHabitsAsync(userId, trackChanges: false);
        return Ok(habits);
    }

    [HttpGet("{id:guid}", Name = "HabitById")]
    public async Task<IActionResult> GetHabit(Guid id, [FromHeader] string authorization)
    {
        var userId = await _service.AuthenticationService.GetUserId(authorization);

        var habit = await _service.HabitService.GetHabitAsync(userId, id, trackChanges: false);

        return Ok(habit);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateHabit([FromBody] HabitForCreationDto habit, [FromHeader] string authorization)
    {
        var userId = await _service.AuthenticationService.GetUserId(authorization);

        var createdHabit = await _service.HabitService.CreateHabitAsync(userId, habit);
        return CreatedAtRoute("HabitById", new { id = createdHabit.Id }, createdHabit);
    }
       
    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateHabit(Guid id, [FromBody] HabitForUpdateDto habit, [FromHeader] string authorization)
    {
        var userId = await _service.AuthenticationService.GetUserId(authorization);

        await _service.HabitService.UpdateHabitAsync(userId, id, habit, trackChanges: true);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteHabit(Guid id, [FromHeader] string authorization)
    {
        var userId = await _service.AuthenticationService.GetUserId(authorization);

        await _service.HabitService.DeleteHabitAsync(userId, id, trackChanges: false);
        return NoContent();
    }
}
