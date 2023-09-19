using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpeakUp.Models;
using SpeakUp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Utilities
{
	public class DbInitializer : IDbInitializer
	{
		private UserManager<ApplicationUser> _userManager;
		private RoleManager<IdentityRole<int>> _roleManager;
		private ApplicationDbContext _context;

		public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
		}

		public void Initialize()
		{
			try
			{
				if (_context.Database.GetPendingMigrations().Count() > 0)
				{
					_context.Database.Migrate();
				}
			}
			catch (Exception)
			{
				throw;
			}
			if (!_roleManager.RoleExistsAsync(WebSiteRole.WebSite_Admin).GetAwaiter().GetResult())
			{
				_roleManager.CreateAsync(new IdentityRole<int>(WebSiteRole.WebSite_Admin)).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole<int>(WebSiteRole.WebSite_Dev)).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole<int>(WebSiteRole.WebSite_Learner)).GetAwaiter().GetResult();

				_userManager.CreateAsync(new ApplicationUser
				{
					UserName = "Thracian14",
					Email = "thracian14@gmail.com"
				}, "testpassword").GetAwaiter().GetResult();
				var appuser = _userManager.Users.Where(x => x.Email == "thracian14@gmail.com").FirstOrDefault();
				if (appuser != null)
				{
					_userManager.AddToRoleAsync(appuser, WebSiteRole.WebSite_Admin).GetAwaiter().GetResult();
				}
			}
		}
	}
}
