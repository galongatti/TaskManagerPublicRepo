using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerBackEnd.Authorize;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;
using TaskManagerBackEnd.Service;

namespace TaskManagerBackEnd.Controllers;

[ApiController]
[Route("[controller]")]
public class AssignmentController(IAssignmentService service, ILogger<AssignmentController> logger) : ControllerBase
{
    [HttpPost("CreateAssignment")]
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
    
    [HttpPut("UpdateAssignment")]
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

    [HttpGet("GetAssignments")]
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

    [HttpGet("GetAssignmentById")]
    [CustomAuthorize(["*"])]
    public ActionResult<Assignment> GetAssignmentById(int id)
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

    [HttpGet("GetAssignmentsTeams")]
    [CustomAuthorize(["*"])]
    public ActionResult<List<Assignment>> GetAssignmentsTeams([FromHeader] int[] idTeams)
    {
        try
        {
            List<Assignment> res = service.GetAssignmentsByTeamsId(idTeams);
            return Ok(res);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
}