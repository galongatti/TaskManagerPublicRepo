using System.Security.Claims;
using src.TaskManagerBackEnd;

namespace TaskManagerBackEnd.Service;

public interface ITokenService
{
    
    string GenerateToken(string email);
    string? ValidateToken(string token);
    ClaimsIdentity GenerateClaimsIdentity(User? user);  
}