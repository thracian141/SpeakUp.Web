using SpeakUp.Models;
using SpeakUp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Services
{
	public interface IDailyPerformanceService
	{
		Task TrackDailyPerformanceAsync(DailyPerformanceViewModel dailyPerformance);
		Task<DailyPerformanceViewModel> GetDailyPerformanceAsync(int userId, DateTime date);
		Task<List<DailyPerformanceViewModel>> GetAllDailyPerformancesAsync(int userId);
		Task Create(ApplicationUser vm);
		Task<DailyPerformanceViewModel> GetTodaysDailyPerformance(int userId);
	}
}

