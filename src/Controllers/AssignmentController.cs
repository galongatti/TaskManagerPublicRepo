using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerBackEnd.Authorize;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;
using TaskManagerBackEnd.Service;

namespace TaskManagerBackEnd.Controllers;

/// <summary>
/// Endpoint for managing assignments
/// </summary>
/// <param name="service"></param>
/// <param name="logger"></param>
[ApiController]
[Route("[controller]")]
public class AssignmentController(IAssignmentService service, ILogger<AssignmentController> logger) : ControllerBase
{
    
    /// <summary>
    /// Endpoint to create assignment
    /// </summary>
    /// <param name="assignmentDTO"></param>
    /// <returns></returns>
    [HttpPost]
    [CustomAuthorize(["Admin", "Developer"])]
    public ActionResult<bool> CreateAssignment([FromBody] AssignmentDTOInsert assignmentDTO)
    {
        try
        {
            if (ModelState.IsValid == false) return BadRequest("Invalid assignment");

            int? res = service.CreateAssignment(assignmentDTO);

            if(res is null) return BadRequest("Assignment not created");

            return Ok(res);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
    /// <summary>
    /// Endpoint to update assignment
    /// </summary>
    /// <param name="assignmetDTO"></param>
    /// <returns></returns>
    
    [HttpPut]
    [CustomAuthorize(["Admin", "Developer"])]
    public ActionResult<bool> UpdateAssignment([FromBody] AssignmentDTOUpdate assignmetDTO)
    {
        try
        {
            if (ModelState.IsValid == false) return BadRequest("Invalid assignment");

            bool res = service.UpdateAssignment(assignmetDTO);

            if (!res) return BadRequest("Assignment not updated");

            return Ok(res);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
    /// <summary>
    /// Endpoint to get all assignments
    /// </summary>
    /// <returns></returns>

    [HttpGet]
    [CustomAuthorize(["*"])]
    public ActionResult<List<Assignment>> GetAssignments()
    {
        try
        {
            if(ModelState.IsValid == false) return BadRequest("Invalid assignment");
            List<Assignment> res = service.GetAssignments();
            
            return Ok(res); 
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }

    /// <summary>
    /// Endpoint to get assignment by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [CustomAuthorize(["*"])]
    public ActionResult<Assignment> GetAssignmentById([FromRoute] int id)
    {
        try
        {
            if(ModelState.IsValid == false) return BadRequest("Invalid assignment");
            Assignment? res = service.GetAssignment(id);
            
            return Ok(res); 
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
    /// <summary>
    /// Endpoint to get assignments by teams
    /// </summary>
    /// <param name="idTeams"></param>
    /// <returns></returns>

    [HttpGet("get-assignments-by-teams")]
    [CustomAuthorize(["*"])]
    public ActionResult<List<Assignment>> GetAssignmentsTeams(int[] idTeams)
    {
        try
        {
            List<Assignment> res = service.GetAssignmentsByTeamsId(idTeams);
            return Ok(res);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
    /// <summary>
    /// Endpoint to get assignments by users
    /// </summary>
    /// <param name="idUsers"></param>
    /// <returns></returns>
    
    [HttpGet("get-assignments-by-users")]
    [CustomAuthorize(["*"])]
    public ActionResult<List<Assignment>> GetAssignmentsByUser(int[] idUsers)
    {
        try
        {
            List<Assignment> res = service.GetAssignmentsByUserId(idUsers);
            return Ok(res);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
    
}