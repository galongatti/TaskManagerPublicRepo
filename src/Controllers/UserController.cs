using Microsoft.AspNetCore.Mvc;
using src.TaskManagerBackEnd.DTO;
using src.TaskManagerBackEnd.Service;

namespace src.TaskManagerBackEnd.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IMemberService _memberService;
    private readonly IConfiguration _conf;

    public UserController(ILogger<UserController> logger, IMemberService memberService, IConfiguration conf)
    {
        _logger = logger;
        _memberService = memberService;
        _conf = conf;
    }

    [HttpPost("CreateUser")]
    public ActionResult<bool> CreateUser([FromBody] UserInsertDTO user)
    {
        try
        {
            if (ModelState.IsValid == false) return BadRequest("Invalid member");

            bool res = _memberService.AddMember(user);

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
    public ActionResult<bool> UpdateUser([FromBody] UserUpdateDTO user)
    {
        try
        {
            if (ModelState.IsValid == false) return BadRequest("Invalid member");

            bool res = _memberService.UpdateMember(user);

            if (!res) return BadRequest("Member not updated");

            return Ok(res);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest("Something went wrong");
        }
    }

    [HttpGet("TestarComunicacao")]
    public ActionResult<string> TestarComunicacao()
    {
        try
        {
            return Ok(_conf["Environment"]);
        }
        catch (Exception)
        {

            return BadRequest(_conf["Environment"]);
        }
    }
}