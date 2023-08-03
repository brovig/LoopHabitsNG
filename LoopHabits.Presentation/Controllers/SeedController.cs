using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace LoopHabits.Presentation.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class SeedController : ControllerBase
{
    private readonly IServiceManager service;

    public SeedController()
    {

    }
}
