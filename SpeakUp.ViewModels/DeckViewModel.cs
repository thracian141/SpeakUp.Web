using Microsoft.AspNetCore.Http;
using SpeakUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.ViewModels
{
	public class DeckViewModel
	{
        public int Id { get; set; }
        public string DeckName { get; set; }
        public string? DeckImage { get; set; }
        public string? DeckDescription { get; set; }
		public int Level { get; set; }
        public int UserId { get; set; }
        public bool IsCourse { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Words>? Words { get; set; }

		public IFormFile? DeckImageFile { get; set; }
		public ICollection<Grammar>? Grammars { get; set; }

		public DeckViewModel()
		{

		}

		public DeckViewModel(Deck deck)
		{
			Id = deck.Id;
			DeckName = deck.DeckName;
			DeckImage = deck.DeckImage;
			DeckDescription = deck.DeckDescription;
			UserId = deck.UserId;
			User = deck.User;
			Words = deck.Words;
			IsCourse = deck.IsCourse;
			Grammars = deck.Grammars;
		}

		public DeckViewModel(DeckViewModel deck) {
			Id = deck.Id;
			DeckName = deck.DeckName;
			DeckImage = deck.DeckImage;
			DeckDescription = deck.DeckDescription;
			UserId = deck.UserId;
			User = deck.User;
			Words = deck.Words;
			IsCourse = deck.IsCourse;
			Grammars = deck.Grammars;
		}

		public Deck ConvertViewModel(DeckViewModel model)
		{
			return new Deck
			{
				Id = model.Id,
				DeckName = model.DeckName,
				DeckImage = model.DeckImage,
				DeckDescription = model.DeckDescription,
				UserId = model.UserId,
				User = model.User,
				Words = model.Words,
				IsCourse = model.IsCourse,
				Grammars = model.Grammars
			};
		}
	}
}
