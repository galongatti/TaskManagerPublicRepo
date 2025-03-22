using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerBackEnd.Authorize;
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
    private readonly ITokenService _tokenService;

    public UserController(ILogger<UserController> logger, IUserService userService, SecretsManager secretManager, ITokenService tokenService)
    {
        _logger = logger;
        _userService = userService;
        _secretManager = secretManager;
        _tokenService = tokenService;
    }

    [HttpPost("CreateUser")]
    [Authorize]
    [CustomAuthorize(["Admin","Developer"])]
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
    [Authorize]
    [CustomAuthorize(["Admin"])]
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
    [Authorize]
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
    [Authorize]
    [CustomAuthorize(["Admin"])]
    public ActionResult<bool> DeleteUser(int idUser)
    {
        try
        {
            bool res = _userService.DeleteUser(idUser);

            if (!res) return BadRequest("Member not deleted");

            return Ok(res);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public ActionResult<string> Login([FromBody] UserLoginDto user)
    {
        try
        {
            if (ModelState.IsValid == false) return BadRequest("Invalid model");

            bool validUser = _userService.CheckPassword(user);

            if (validUser == false) return BadRequest("User or password invalid");

            string token = _tokenService.GenerateToken(user.Email);
            
            return Ok(token);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }

    [HttpGet("TestRequest")]
    [Authorize]
    [CustomAuthorize(["Admin", "Developer"])]
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