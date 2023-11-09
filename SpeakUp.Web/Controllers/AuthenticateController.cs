using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpeakUp.Models;
using SpeakUp.Services;
using SpeakUp.Web.Models;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase {

    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AuthController> _logger;
    private readonly ITokenService _tokenService;

    public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<AuthController> logger
    , ITokenService tokenService) {
        _tokenService = tokenService;
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model) {
        // validate the model
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        // authenticate the user
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password)) {
            return Unauthorized();
        }

        // generate a token
        var token = await _tokenService.GenerateToken(user);

        // create a cookie with the token
        var cookieOptions = new CookieOptions {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("token", token, cookieOptions);

        // return the token as JSON
        return new JsonResult(new { token });
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout() {
        await _signInManager.SignOutAsync();
        Response.Cookies.Delete("token");
        _logger.LogInformation("User logged out.");

        return Ok();
    }
}