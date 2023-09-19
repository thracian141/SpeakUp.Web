using SpeakUp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.ViewModels
{
	public class WordViewModel
	{
		public int Id { get; set; }
		public string Front { get; set; }
		public string Back { get; set; }
		public int Level { get; set; }
		public int Difficulty { get; set; }
		[DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = true)]
		public DateTime DateAdded { get; set; }
		[DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = true)]
		public DateTime LastReviewDate { get; set; }
		[DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = true)]
		public DateTime NextReviewDate { get; set; }
		public string? Description { get; set; }
		public int DeckId { get; set; }
		public Deck? Deck { get; set; }
		public int? GrammarId { get; set; }
		public Grammar? Grammar { get; set; }

		public WordViewModel()
		{

		}

		public WordViewModel(Words word)
		{
			Id = word.Id;
			Front = word.Front;
			Back = word.Back;
			Level = word.Level;
			Difficulty = word.Difficulty;
			DateAdded = word.DateAdded;
			LastReviewDate = word.LastReviewDate;
			NextReviewDate = word.NextReviewDate;
			Description = word.Description;
			DeckId = word.DeckId;
			Deck = word.Deck;
			GrammarId = word.GrammarId;
			Grammar = word.Grammar;
		}

		public WordViewModel(WordViewModel word) {
			Id = word.Id;
			Front = word.Front;
			Back = word.Back;
			Level = word.Level;
			Difficulty = word.Difficulty;
			DateAdded = word.DateAdded;
			LastReviewDate = word.LastReviewDate;
			NextReviewDate = word.NextReviewDate;
			Description = word.Description;
			DeckId = word.DeckId;
			Deck = word.Deck;
			GrammarId = word.GrammarId;
			Grammar = word.Grammar;
		}

		public Words ConvertViewModel(WordViewModel model)
		{
			return new Words
			{
				Id = model.Id,
				Front = model.Front,
				Back = model.Back,
				Level = model.Level,
				Difficulty = model.Difficulty,
				DateAdded = model.DateAdded,
				LastReviewDate = model.LastReviewDate,
				NextReviewDate = model.NextReviewDate,
				Description = model.Description,
				DeckId = model.DeckId,
				Deck = model.Deck,
				GrammarId = model.GrammarId,
				Grammar = model.Grammar
			};
		}
	}
}
