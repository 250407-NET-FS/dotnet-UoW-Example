using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepoWithUoW.Domain;

namespace RepoWithUoW.Service;

public class AuthService_Impl : IAuthService
{

    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _config;

    public AuthService_Impl(UserManager<User> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    } 


    public async Task<string> GenerateToken(User u)
    {

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, u.UserName),
            new Claim(ClaimTypes.NameIdentifier, u.Id),
            new Claim(ClaimTypes.Email, u.Email)
        };

        var roles = await _userManager.GetRolesAsync(u);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var jwtKey = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt Key not configured");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(Double.Parse(_config["Jwt:ExpireDays"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}