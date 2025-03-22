using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using src.TaskManagerBackEnd;

namespace TaskManagerBackEnd.Service;

public class TokenService(IConfiguration config, IUserService userService) : ITokenService
{
    public string GenerateToken(string email)
    {
        User? user = userService.GetUserByEmail(email);
        
        if (user is null)
            throw new Exception("User not found");

        JwtSecurityTokenHandler handler = new();

        byte[] key = Encoding.ASCII.GetBytes(config["JwtKey"]);
        SymmetricSecurityKey symmetricAlgorithm = new(key);
        SigningCredentials signingCredentials = new(symmetricAlgorithm, SecurityAlgorithms.HmacSha256Signature);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            SigningCredentials = signingCredentials,
            Expires = DateTime.UtcNow.AddHours(2),
            Subject = GenerateClaimsIdentity(user),
            Issuer = "TaskManager",
            Audience = "TaskManager",
        };

        SecurityToken token = handler.CreateToken(tokenDescriptor);

        string strToken = handler.WriteToken(token);
        return strToken;
    }

    public string? ValidateToken(string token)
    {
        JwtSecurityTokenHandler handler = new();
        byte[] key = Encoding.ASCII.GetBytes(config["JwtKey"]);

        try
        {
            handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = "TaskManager",
                ValidateAudience = true,
                ValidAudience = "TaskManager",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            JwtSecurityToken? jwtToken = (JwtSecurityToken)validatedToken;
            return Convert.ToString(jwtToken.Payload["email"]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public ClaimsIdentity GenerateClaimsIdentity(User user)
    {
        ClaimsIdentity ci = new();
        ci.AddClaim(new Claim(ClaimTypes.Name, user.Name));
        ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        ci.AddClaim(new Claim(ClaimTypes.Role, user.Post));
        return ci;
    }
}