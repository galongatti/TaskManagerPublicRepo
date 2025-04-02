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
public class UserController(ILogger<UserController> logger, IUserService userService, ITokenService tokenService)
    : ControllerBase
{
    [HttpPost("CreateUser")]
    [Authorize]
    [CustomAuthorize(["Admin"])]
    public ActionResult<bool> CreateUser([FromBody] UserInsertDTO user)
    {
        try
        {
            if (ModelState.IsValid == false) return BadRequest("Invalid member");

            bool res = userService.AddUser(user);

            if (!res) return BadRequest("Member not created");

            return Ok(res);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
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

            bool res = userService.UpdateUser(user);

            if (!res) return BadRequest("Member not updated");

            return Ok(res);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }

    [HttpPut("UpdatePassword")]
    [CustomAuthorize(["*"])]
    public ActionResult<bool> UpdatePassword([FromBody] UserUpdatePasswordDto user)
    {
        try
        {
            if (ModelState.IsValid == false) return BadRequest("Invalid member");

            bool res = userService.UpdatePassword(user);

            if (!res) return BadRequest("Password not updated");

            return Ok(res);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }

    [HttpDelete("DeleteUser")]
    [CustomAuthorize(["Admin"])]
    public ActionResult<bool> DeleteUser(int idUser)
    {
        try
        {
            bool res = userService.DeleteUser(idUser);

            if (!res) return BadRequest("Member not deleted");

            return Ok(res);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
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

            bool validUser = userService.CheckPassword(user);

            if (validUser == false) return BadRequest("User or password invalid");

            string token = tokenService.GenerateToken(user.Email);
            
            return Ok(token);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }
}