using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpeakUp.Models;
using SpeakUp.Web.Models;

[Route("api")]
[ApiController]
public class AuthController : ControllerBase {
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AuthController> _logger;

    public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<AuthController> logger) {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }
    [HttpGet("test")]
    public IActionResult Test() {
        return Ok("Hello, world");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model) {
        _logger.LogInformation("Received a login request");
        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded) {
            return Ok(new { message = "Login successful" });
        }

        await Console.Out.WriteLineAsync(":( !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        return BadRequest(new { message = "Invalid login attempt" });
    }
}