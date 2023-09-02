using LoopHabits.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

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
    public async Task<IActionResult> GetRepetitions(Guid habitId, [FromQuery] RepetitionParameters repetitionParameters)
    {
        var repetitions = await _service.RepetitionService.GetRepetitionsAsync(habitId, repetitionParameters, trackChanges: false);
        return Ok(repetitions);
    }

    [HttpGet("{id:int}", Name = "GetRepetitionForHabit")]
    public async Task<IActionResult> GetRepetition(Guid habitId, int id)
    {
        var repetition = await _service.RepetitionService.GetRepetitionAsync(habitId, id, trackChanges: false);
        return Ok(repetition);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateRepetition(Guid habitId, [FromBody] RepetitionForCreationDto repetition)
    {
        var repetitionToReturn = await _service.RepetitionService.CreateRepetitionAsync(habitId, repetition, trackChanges: false);
        return CreatedAtRoute("GetRepetitionForHabit", new { habitId, id = repetitionToReturn.Id }, repetitionToReturn);
    }

    [HttpPut("{id:int}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateRepetition(Guid habitId, int id, [FromBody] RepetitionForUpdateDto repetition)
    {
        await _service.RepetitionService.UpdateRepetitionAsync(habitId, id, repetition, habitTrackChanges: false, repetitionTrackChanges: true);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRepetition(Guid habitId, int id)
    {
        await _service.RepetitionService.DeleteRepetitionAsync(habitId, id, trackChanges: false);
        return NoContent();
    }
}
