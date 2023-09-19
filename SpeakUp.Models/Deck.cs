using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Models
{
	public class Deck
	{
		public int Id { get; set; }
		public string DeckName { get; set; }
		public int Level { get; set; }
		public int Difficulty { get; set; }
		public string? DeckImage { get; set; }
		public string? DeckDescription { get; set; }
		public bool IsCourse { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
		public ICollection<Words>? Words { get; set; }
		public ICollection<Grammar>? Grammars { get; set; }
	}
}
