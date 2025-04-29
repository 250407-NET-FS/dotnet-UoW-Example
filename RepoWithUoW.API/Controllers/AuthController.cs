
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RepoWithUoW.Domain;
using RepoWithUoW.Service;

namespace RepoWithUoW.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

    private readonly UserManager<User> _userManager;
    private readonly IAuthService _authService;

    public AuthController(UserManager<User> userManager, IAuthService authService)
    {
        _userManager = userManager;
        _authService = authService;
    }

    [EnableCors("Development")]
    [Authorize(Roles = "Admin")]
    [HttpPost("registeradmin")]
    public async Task<IActionResult> Register(RegisterDTO newUser)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);

        var user = new User
        {
            UserName = newUser.Email,
            Email = newUser.Email,
            FullName = newUser.FullName
        };

        var result = await _userManager.CreateAsync(user, newUser.Password);

        if(!result.Succeeded) return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, "Admin");
        return Ok(new { message = "Registration Successful" });
    }

    [EnableCors("Development")]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO existingUser)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(existingUser.Email);

        if(user == null || !await _userManager.CheckPasswordAsync(user, existingUser.Password))
        {
            return Unauthorized("Invalid Credentials!");
        }

        var token = await _authService.GenerateToken(user);
        // chrome.exe --user-data-dir="C://Chrome dev session" --disable-web-security
        // if in dev environment, we need to open chrome with CORS disabled to get this to work 
        Response.Cookies.Append("AuthToken", token, new CookieOptions
        {
            HttpOnly = true, // prevents access via JS
            Secure = true, // ensures cookie is sent over HTTPS
            SameSite = SameSiteMode.Strict, // prevent cross-site requests, should be on strict normally
            Expires = DateTimeOffset.UtcNow.AddHours(1) // set expiration time 
        });

        return Ok(new { message = "Login successful" });
    }
}
