using Microsoft.AspNetCore.Mvc;
using TaskManagerBackEnd.Config;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Service;

namespace TaskManagerBackEnd.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private readonly SecretsManager _secretManager;

    public UserController(ILogger<UserController> logger, IUserService userService, SecretsManager secretManager)
    {
        _logger = logger;
        _userService = userService;
       _secretManager = secretManager;
    }

    [HttpPost("CreateUser")]
    public ActionResult<bool> CreateUser([FromBody] UserInsertDTO user)
    {
        try
        {
            if (ModelState.IsValid == false) return BadRequest("Invalid member");

            bool res = _userService.AddMember(user);

            if (!res) return BadRequest("Member not created");

            return Ok(res);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }

    [HttpPut("UpdateUser")]
    public ActionResult<bool> UpdateUser([FromBody] UserUpdateDto user)
    {
        try
        {
            if (ModelState.IsValid == false) return BadRequest("Invalid member");

            bool res = _userService.UpdateMember(user);

            if (!res) return BadRequest("Member not updated");

            return Ok(res);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }

    [HttpPut("UpdatePassword")]
    public ActionResult<bool> UpdatePassword([FromBody] UserUpdatePasswordDto user)
    {
        try
        {
            if (ModelState.IsValid == false) return BadRequest("Invalid member");

            bool res = _userService.UpdatePassword(user);

            if (!res) return BadRequest("Password not updated");

            return Ok(res);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }

    [HttpDelete("DeleteUser")]
    public ActionResult<bool> DeleteUser(int idUser)
    {
        try
        {
            bool res = _userService.DeleteMember(idUser);

            if (!res) return BadRequest("Member not deleted");

            return Ok(res);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
    
    [HttpGet("TestRequest")]
    public ActionResult<string> TestRequest()
    {
        try
        {
            return Ok(_secretManager.GetConnectionStringAsync());

        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
}