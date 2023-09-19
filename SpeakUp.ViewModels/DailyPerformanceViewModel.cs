using SpeakUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.ViewModels
{
	public class DailyPerformanceViewModel
	{
		public DateTime Date { get; set; }
		public int WordsGuessedCount { get; set; }
		public int WordsLearnedCount { get; set; }
		public int MinutesSpentLearning { get; set; }
		public int UserId { get; set; }
		public ApplicationUser User { get; set; }


		public DailyPerformanceViewModel()
		{

		}
		public DailyPerformanceViewModel(DailyPerformance dP)
		{
			Date = dP.Date;
			WordsLearnedCount = dP.WordsLearnedCount;
			WordsGuessedCount = dP.WordsGuessedCount;
			MinutesSpentLearning = dP.MinutesSpentLearning;
			User = dP.User;
			UserId = dP.UserId;
		}

		public DailyPerformance ConvertViewModel(DailyPerformanceViewModel model)
		{
			return new DailyPerformance
			{
				Date = model.Date,
				WordsGuessedCount = model.WordsGuessedCount,
				WordsLearnedCount= model.WordsLearnedCount,
				MinutesSpentLearning= model.MinutesSpentLearning,
				User = model.User,
				UserId = model.UserId				
			};
		}
	}
}
