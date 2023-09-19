using SpeakUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.ViewModels
{
	public class ApplicationUserViewModel
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string? DisplayName { get; set; }
		public string? ProfilePictureUrl { get; set; }
		public DateTime AccountCreatedDate { get; set; }
		public ICollection<Deck>? Decks { get; set; }
		public int? LastDeck { get; set; }

		public ApplicationUserViewModel()
		{

		}

		public ApplicationUserViewModel(ApplicationUser user)
		{
			Id = user.Id;
			UserName = user.UserName;
			DisplayName = user.DisplayName;
			ProfilePictureUrl = user.ProfilePictureUrl;
			AccountCreatedDate = user.AccountCreatedDate;
			Decks = user.Decks;
			LastDeck = user.LastDeck;
		}
		public ApplicationUser ConvertViewModelToModel(ApplicationUserViewModel user)
		{
			return new ApplicationUser
			{
				Id = user.Id,
				UserName = user.UserName,
				DisplayName = user.DisplayName,
				ProfilePictureUrl = user.ProfilePictureUrl,
				AccountCreatedDate = user.AccountCreatedDate,
				Decks = user.Decks,
				LastDeck = user.LastDeck
			};
		}
	}
}
