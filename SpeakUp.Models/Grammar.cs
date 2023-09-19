using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Models
{
	public class Grammar
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
	}
}
