using SpeakUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.ViewModels
{
	public class GrammarViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Level { get; set; }
		public DateTime? LastReviewDate { get; set; }
		public DateTime? NextReviewDate { get; set; }
		public string? GrammarUrl { get; set; }
		public int DeckId { get; set; }
		public Deck Deck { get; set; }
		public ICollection<Words>? Words { get; set; }

		public GrammarViewModel()
		{

		}

		public GrammarViewModel(GrammarViewModel grammar)
		{
			Id = grammar.Id;
			Name = grammar.Name;
			LastReviewDate = grammar.LastReviewDate;
			NextReviewDate = grammar.NextReviewDate;
			GrammarUrl = grammar.GrammarUrl;
			DeckId = grammar.DeckId;
			Deck = grammar.Deck;
			Words = grammar.Words;
		}

		public GrammarViewModel(Grammar grammar) {
			Id = grammar.Id;
			Name = grammar.Name;
			LastReviewDate = grammar.LastReviewDate;
			NextReviewDate = grammar.NextReviewDate;
			GrammarUrl = grammar.GrammarUrl;
			DeckId = grammar.DeckId;
			Deck = grammar.Deck;
			Words = grammar.Words;
		}

		public Grammar ConvertViewModel(GrammarViewModel model)
		{
			return new Grammar
			{
				Id = model.Id,
				Name = model.Name,
				LastReviewDate = model.LastReviewDate,
				NextReviewDate = model.NextReviewDate,
				GrammarUrl = model.GrammarUrl,
				DeckId = model.DeckId,
				Deck = model.Deck,
				Words = model.Words
			};
		}
	}
}
