using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerBackEnd.Authorize;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;
using TaskManagerBackEnd.Service;

namespace TaskManagerBackEnd.Controllers;


/// <summary>
/// Controller for team management
/// </summary>
[ApiController]
[Route("[controller]")]
public class TeamController(ILogger<TeamController> logger, ITeamService teamService) : ControllerBase
{
    /// <summary>
    /// Create a new team
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    [HttpPost]
    [CustomAuthorize]
    public ActionResult<bool> CreateTeam([FromBody] TeamInsertDto team)
    {
        try
        {
            if (ModelState.IsValid == false) return BadRequest("Invalid team");

            bool res = teamService.AddTeam(team);

            if (!res) return BadRequest("Team not created");

            return Ok(res);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
    /// <summary>
    /// Endpoint for update team
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    [HttpPut]
    [CustomAuthorize]
    public ActionResult<bool> UpdateTeam([FromBody] TeamUpdateDto team)
    {
        try
        {
            if (ModelState.IsValid == false) return BadRequest("Invalid team");

            bool res = teamService.UpdateTeam(team);

            if (!res) return BadRequest("Team not updated");

            return Ok(res);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
    /// <summary>
    /// Endpoint for get all teams
    /// </summary>
    /// <returns></returns>

    [HttpGet]
    [CustomAuthorize]
    public ActionResult<List<Team>> GetTeams()
    {
        try
        {
            List<Team> teams = teamService.GetTeams();

            return Ok(teams);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
    /// <summary>
    /// Endpoint to get team by id
    /// </summary>
    /// <param name="idTeam"></param>
    /// <returns></returns>
    
    [HttpDelete("{idTeam}")]
    [CustomAuthorize]
    public ActionResult<List<Team>> DeleteTeam(int idTeam)
    {
        try
        {
            bool res  = teamService.DeleteTeam(idTeam);
            return Ok(res);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
}