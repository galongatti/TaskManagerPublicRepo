using src.TaskManagerBackEnd;
using TaskManagerBackEnd.Service;

namespace TaskManagerBackEnd.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITokenService _tokenService;

    public JwtMiddleware(RequestDelegate next, ITokenService tokenService)
    {
        _next = next;
        _tokenService = tokenService;
    }

    public async Task Invoke(HttpContext context)
    {
        string? token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        if (token is not null)
        {
            string? email = _tokenService.ValidateToken(token);

            if (email is not null)
            {
                context.Items["User"] = email;
            }
        }
        
        await _next(context);
    }
}