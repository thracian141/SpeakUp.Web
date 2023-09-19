// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpeakUp.Models;
using SpeakUp.Utilities;

namespace SpeakUp.Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IWebHostEnvironment _env;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }

        public string UserName { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Display name")]
            public string DisplayName { get; set; }
            [DataType(DataType.Password)]
            public string OldPassword { get; set; }
            [DataType(DataType.Password)]
            public string NewPassword { get; set; }
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }
            [EmailAddress]
            [Display(Name = "New email")]
            public string Email { get; set; }
            public IFormFile ProfilePictureUrl { get; set; }
            public IFormFile CroppedProfilePicture { get; set; }

        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var displayName = user.DisplayName;
            var email = await _userManager.GetEmailAsync(user);


            UserName = await _userManager.GetUserNameAsync(user);

            Input = new InputModel
            {
                DisplayName = displayName,
                Email = email
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (Input.Email != user.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    StatusMessage = "Unexpected error occurred setting email.";
                    return RedirectToPage();
                }
            }

            if (!string.IsNullOrEmpty(Input.OldPassword) && !string.IsNullOrEmpty(Input.NewPassword) && !string.IsNullOrEmpty(Input.ConfirmPassword))
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }
            }

            if (Input.DisplayName != user.DisplayName)
            {
                user.DisplayName = Input.DisplayName;
            }

            //ImageOperationsPfp image = new ImageOperationsPfp(_env);
            //string filename = await image.ImageUpload(Input.ProfilePictureUrl);
            //user.ProfilePictureUrl = filename;

            if (Input.CroppedProfilePicture != null)
            {
                ImageOperationsPfp image = new ImageOperationsPfp(_env);

                // Delete the old profile picture before saving the new one
                image.DeleteOldProfilePicture(user.ProfilePictureUrl);

                // Save the cropped image
                string fileExtension = Path.GetExtension(Input.CroppedProfilePicture.FileName);
                string filename = await image.SaveCroppedImage(Input.CroppedProfilePicture, fileExtension);

                // Update the user's profile picture URL with the new filename or path.
                user.ProfilePictureUrl = filename;
            }


            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
