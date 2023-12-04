using LoopHabits.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Net;

namespace LoopHabits.Presentation.Controllers;

[Route("api/habits/{habitId}/repetitions")]
[ApiController]
[Authorize]
public class RepetitionsController : ControllerBase
{
    private readonly IServiceManager _service;

    public RepetitionsController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetRepetitions(Guid habitId, [FromQuery] RepetitionParameters repetitionParameters, [FromHeader] string authorization)
    {
        var userId = await _service.AuthenticationService.GetUserId(authorization);

        var repetitions = await _service.RepetitionService.GetRepetitionsAsync(userId, habitId, repetitionParameters, trackChanges: false);
        return Ok(repetitions);
    }

    [HttpGet("{id:int}", Name = "GetRepetitionForHabit")]
    public async Task<IActionResult> GetRepetition(Guid habitId, int id, [FromHeader] string authorization)
    {
        var userId = await _service.AuthenticationService.GetUserId(authorization);

        var repetition = await _service.RepetitionService.GetRepetitionAsync(userId, habitId, id, trackChanges: false);
        return Ok(repetition);
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetHabitStatistics(Guid habitId, [FromQuery] RepetitionParameters repetitionParameters, [FromHeader] string authorization)
    {
        var userId = await _service.AuthenticationService.GetUserId(authorization);

        var stats = await _service.RepetitionService.GetHabitStatisticsAsync(userId, habitId, repetitionParameters, trackChanges: false);

        return Ok(stats);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateRepetition(Guid habitId, [FromBody] RepetitionForCreationDto repetition, [FromHeader] string authorization)
    {
        var userId = await _service.AuthenticationService.GetUserId(authorization);

        var repetitionToReturn = await _service.RepetitionService.CreateRepetitionAsync(userId, habitId, repetition, trackChanges: false);
        return CreatedAtRoute("GetRepetitionForHabit", new { habitId, id = repetitionToReturn.Id }, repetitionToReturn);
    }

    [HttpPut("{id:int}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateRepetition(Guid habitId, int id, [FromBody] RepetitionForUpdateDto repetition, [FromHeader] string authorization)
    {
        var userId = await _service.AuthenticationService.GetUserId(authorization);

        await _service.RepetitionService.UpdateRepetitionAsync(userId, habitId, id, repetition, habitTrackChanges: false, repetitionTrackChanges: true);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRepetition(Guid habitId, int id, [FromHeader] string authorization)
    {
        var userId = await _service.AuthenticationService.GetUserId(authorization);

        await _service.RepetitionService.DeleteRepetitionAsync(userId, habitId, id, trackChanges: false);
        return NoContent();
    }
}
