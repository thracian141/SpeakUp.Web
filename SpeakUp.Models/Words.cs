namespace SpeakUp.Models
{
	public class Words
	{
		public int Id { get; set; }
		public string Front { get; set; }
		public string Back { get; set; }
		public int Level { get; set; }
		public int Difficulty { get; set; }
		public DateTime DateAdded { get; set; }
		public DateTime LastReviewDate { get; set; }
		public DateTime NextReviewDate { get; set; }
		public string? Description { get; set; }
		public int DeckId { get; set; }
		public Deck Deck { get; set; }
		public int? GrammarId { get; set; }
		public Grammar? Grammar { get; set; }
		public ICollection<Sentence>? Sentences { get; set; }
	}
}