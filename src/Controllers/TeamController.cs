using Microsoft.AspNetCore.Mvc;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Service;

namespace TaskManagerBackEnd.Controllers;


[ApiController]
[Route("[controller]")]
public class TeamController : ControllerBase
{
    private readonly ILogger<TeamController> _logger;
    private readonly ITeamService _teamService;

    public TeamController(ILogger<TeamController> logger, ITeamService teamService)
    {
        _logger = logger;
        _teamService = teamService;
    }
    
    [HttpPost("CreateTeam")]
    public ActionResult<bool> CreateTeam([FromBody] TeamInsertDto team)
    {
        try
        {
            if (ModelState.IsValid == false) return BadRequest("Invalid team");

            bool res = _teamService.AddTeam(team);

            if (!res) return BadRequest("Team not created");

            return Ok(res);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
}