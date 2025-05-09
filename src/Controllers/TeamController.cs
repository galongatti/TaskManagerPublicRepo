using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerBackEnd.Authorize;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Mappers;
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
            List<TeamDto> teamDtos = teams.Select(x => x.MapToGetDto()).ToList();

            return Ok(teamDtos);
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
    /// <param name="idTeam">Id of the team to be delete</param>
    /// <returns></returns>
    [HttpDelete("{idTeam}")]
    [CustomAuthorize]
    public ActionResult<bool> DeleteTeam(int idTeam)
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
    
    /// <summary>
    /// Endpoint to search the Team by ID.
    /// </summary>
    /// <param name="idTeam">Id of the team to be searched</param>
    /// <returns>
    /// Retorna HTTP 200 com os detalhes do time correspondente ao ID fornecido.
    /// Retorna HTTP 400 se ocorrer um erro ou se o ID for inválido.
    /// </returns>
    [HttpGet("{idTeam}")]
    [CustomAuthorize]
    public ActionResult<TeamDto> GetTeamById(int idTeam)
    {
        try
        {
            Team? team = teamService.GetTeamById(idTeam);

            if (team is null) return BadRequest("Team not found");

            TeamDto teamDto = team.MapToGetDto();
            return Ok(teamDto);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
    
    
}