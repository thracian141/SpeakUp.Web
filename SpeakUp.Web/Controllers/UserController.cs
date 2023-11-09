using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpeakUp.Models;

namespace SpeakUp.Web.Controllers {
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager) {
            _userManager = userManager;
        }

        [HttpGet("userinfo")]
        public async Task<IActionResult> GetProfile() {
            var user = await _userManager.GetUserAsync(User);

            if (user != null) {
                var userProfile = new {
                    UserName = user.UserName,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    AccountCreatedDate = user.AccountCreatedDate,
                    LastDeck = user.LastDeck
                };

                return Ok(userProfile);
            }

            return NotFound("User not found");
        }
    }
}
