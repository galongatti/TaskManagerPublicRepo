using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerBackEnd.Authorize;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;
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
    [CustomAuthorize(["Admin"])]
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
    
    [HttpPut("UpdateTeam")]
    [CustomAuthorize(["Admin"])]
    public ActionResult<bool> UpdateTeam([FromBody] TeamUpdateDto team)
    {
        try
        {
            if (ModelState.IsValid == false) return BadRequest("Invalid team");

            bool res = _teamService.UpdateTeam(team);

            if (!res) return BadRequest("Team not updated");

            return Ok(res);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }

    [HttpGet("GetTeams")]
    [CustomAuthorize(["*"])]
    public ActionResult<List<Team>> GetTeams()
    {
        try
        {
            List<Team> teams = _teamService.GetTeams();

            return Ok(teams);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
    [HttpDelete("DeleteTeam")]
    [CustomAuthorize(["Admin"])]
    public ActionResult<List<Team>> DeleteTeam(int idTeam)
    {
        try
        {
            bool res  = _teamService.DeleteTeam(idTeam);
            return Ok(res);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
}