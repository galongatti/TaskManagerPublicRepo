using Microsoft.AspNetCore.Mvc;
using TaskManagerBackEnd.Authorize;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Mappers;
using TaskManagerBackEnd.Model;
using TaskManagerBackEnd.Service;

namespace TaskManagerBackEnd.Controllers;

/// <summary>
/// Endpoint for managing assignments
/// </summary>
/// <param name="assignmentService"></param>
/// <param name="logger"></param>
[ApiController]
[Route("[controller]")]
public class AssignmentController(IAssignmentService assignmentService, ILogger<AssignmentController> logger) : ControllerBase
{
    
    /// <summary>
    /// Creates a new assignment.
    /// </summary>
    /// <param name="assignmentPostDto">The data transfer object containing the details of the assignment to be created.</param>
    /// <returns>
    /// Returns HTTP 200 with the ID of the created assignment if successful.
    /// Returns HTTP 400 if the assignment is not created or if the input data is invalid.
    /// </returns>
    [HttpPost]
    [CustomAuthorize]
    public ActionResult<bool> CreateAssignment([FromBody] AssignmentPostDto assignmentPostDto)
    {
        try
        {
            if (ModelState.IsValid == false) return BadRequest("Invalid assignment");

            Assignment? res = assignmentService.CreateAssignment(assignmentPostDto);
            
            if(res is null) return BadRequest("Assignment not created");
            
            AssignmentDto assignmentDto = res.MapModelToDto();
            return Ok(assignmentDto);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
    /// <summary>
    /// Updates an existing assignment.
    /// </summary>
    /// <param name="assignmetPutDto">The data transfer object containing the updated details of the assignment.</param>
    /// <returns>
    /// Returns HTTP 200 if the assignment is successfully updated.
    /// Returns HTTP 400 if the assignment is not updated or if the input data is invalid.
    /// </returns>
    [HttpPut]
    [CustomAuthorize]
    public ActionResult<AssignmentDto> UpdateAssignment([FromBody] AssignmentPutDto assignmetPutDto)
    {
        try
        {
            if (ModelState.IsValid == false) return BadRequest("Invalid assignment");

            Assignment? res = assignmentService.UpdateAssignment(assignmetPutDto);

            if (res is null) return BadRequest("Assignment not updated");
            AssignmentDto assignmentDto = res.MapModelToDto();

            return Ok(assignmentDto);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
    /// <summary>
    /// Retrieves all assignments.
    /// </summary>
    /// <returns>
    /// Returns HTTP 200 with a list of all assignments.
    /// Returns HTTP 400 if an error occurs while retrieving the assignments.
    /// </returns>
    [HttpGet]
    [CustomAuthorize]
    public ActionResult<List<Assignment>> GetAssignments()
    {
        try
        {
            if(ModelState.IsValid == false) return BadRequest("Invalid assignment");
            List<Assignment> res = assignmentService.GetAssignments();
            List<AssignmentDto> assignmentDto = res.Select(a => a.MapModelToDto()).ToList();
            return Ok(assignmentDto); 
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }

    /// <summary>
    /// Retrieves a specific assignment by its ID.
    /// </summary>
    /// <param name="id">The ID of the assignment to retrieve.</param>
    /// <returns>
    /// Returns HTTP 200 with the details of the assignment corresponding to the provided ID.
    /// Returns HTTP 400 if an error occurs or if the ID is invalid.
    /// </returns>
    [HttpGet("{id}")]
    [CustomAuthorize]
    public ActionResult<Assignment> GetAssignmentById([FromRoute] int id)
    {
        try
        {
            if(ModelState.IsValid == false) return BadRequest("Invalid assignment");
            Assignment? res = assignmentService.GetAssignment(id);
            AssignmentDto assignmentDto = (res ?? throw new Exception("Assignment not found")).MapModelToDto();
            
            return Ok(assignmentDto); 
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
    /// <summary>
    /// Retrieves assignments associated with specific teams.
    /// </summary>
    /// <param name="idTeams">An array of team IDs whose assignments should be retrieved.</param>
    /// <returns>
    /// Returns HTTP 200 with a list of assignments associated with the specified teams.
    /// Returns HTTP 400 if an error occurs while retrieving the assignments.
    /// </returns>
    [HttpGet("get-assignments-by-teams")]
    [CustomAuthorize]
    public ActionResult<List<Assignment>> GetAssignmentsTeams([FromQuery] int[] idTeams)
    {
        try
        {
            List<Assignment> res = assignmentService.GetAssignmentsByTeamsId(idTeams);
            List<AssignmentDto> assignmentDto = res.Select(a => a.MapModelToDto()).ToList();
            return Ok(assignmentDto);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
    /// <summary>
    /// Retrieves assignments associated with specific users.
    /// </summary>
    /// <param name="idUsers">An array of user IDs whose assignments should be retrieved.</param>
    /// <returns>
    /// Returns HTTP 200 with a list of assignments associated with the specified users.
    /// Returns HTTP 400 if an error occurs while retrieving the assignments.
    /// </returns>
    [HttpGet("get-assignments-by-users")]
    [CustomAuthorize]
    public ActionResult<List<Assignment>> GetAssignmentsByUser([FromQuery] int[] idUsers)
    {
        try
        {
            List<Assignment> res = assignmentService.GetAssignmentsByUserId(idUsers);
            List<AssignmentDto> assignmentDto = res.Select(a => a.MapModelToDto()).ToList();
            return Ok(assignmentDto);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
    /// <summary>
    /// Deletes an assignment by its ID.
    /// </summary>
    /// <param name="id">The ID of the assignment to delete.</param>
    /// <returns>
    /// Returns HTTP 200 if the assignment is successfully deleted.
    /// Returns HTTP 400 if the assignment is not found, cannot be deleted, or if an error occurs.
    /// </returns>
    [HttpDelete("{id}")]
    [CustomAuthorize]
    public ActionResult<bool> DeleteAssignment([FromRoute] int id)
    {
        try
        {
            bool result = assignmentService.DeleteAssignment(id);

            if (!result) return BadRequest("Assignment not deleted");

            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
    
}