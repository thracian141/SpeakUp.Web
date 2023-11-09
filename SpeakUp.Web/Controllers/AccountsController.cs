using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SpeakUp.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace SpeakUp.Web.Controllers {
    [Route("account")]
    [ApiController]
    public class AccountsController : Controller {
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountsController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env) {
            _env = env;
            _userManager = userManager;
        }
        [HttpGet("userinfo")]
        public async Task<IActionResult> Userinfo() {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) {
                return NotFound("User not found");
            }
            // create an anonymous object with the user's information
            var userInfo = new {
                user.Id,
                user.UserName,
                user.Email,
                user.DisplayName,
                user.ProfilePictureUrl,
                user.AccountCreatedDate,
                user.LastDeck
            };

            return new JsonResult(new { userInfo });
        }

        [HttpGet("profilepicture")]
        public async Task<IActionResult> ProfilePicture(string profilePictureUrl) {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) {
                return NotFound("User not found");
            }

            var path = Path.Combine(_env.WebRootPath, "ImagesProfiles", profilePictureUrl);
            var image = System.IO.File.OpenRead(path);

            // check if the file exists, if it doesn't return a default image
            if (!System.IO.File.Exists(path)) {
                path = Path.Combine(_env.WebRootPath, "ImagesProfiles", "default.png");
                image = System.IO.File.OpenRead(path);
            }

            return File(image, "image/jpeg");
        }
    }
}
