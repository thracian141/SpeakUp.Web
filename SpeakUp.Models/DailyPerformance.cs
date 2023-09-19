using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Models
{
	public class DailyPerformance
	{
		public DateTime Date { get; set; }
		public int WordsGuessedCount { get; set; }
		public int WordsLearnedCount { get; set; }
		public int MinutesSpentLearning { get; set; }
		public int UserId { get; set; }
		public ApplicationUser User { get; set; }
	}
}
