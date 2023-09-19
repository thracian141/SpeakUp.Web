using Microsoft.EntityFrameworkCore;
using SpeakUp.Models;
using SpeakUp.Repository;
using SpeakUp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpeakUp.Services {
	public class DailyPerformanceService : IDailyPerformanceService {
		private IUnitOfWork _unitOfWork;
		public DailyPerformanceService(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork;
		}

		public async Task Create(ApplicationUser vm) {;
			var existingPerformance = await _unitOfWork.GenericRepository<DailyPerformance>()
				.GetByCompositeIdAsync(dp => dp.UserId == vm.Id && dp.Date == DateTime.Now.Date);

			if (existingPerformance == null) {
				var model = new DailyPerformance {
					Date = DateTime.Now.Date,
					WordsGuessedCount = 0,
					WordsLearnedCount = 0,
					MinutesSpentLearning = 0,
					UserId = vm.Id
				};
				_unitOfWork.GenericRepository<DailyPerformance>().Add(model);
				await _unitOfWork.Save();
			}
		}

		public async Task<List<DailyPerformanceViewModel>> GetAllDailyPerformancesAsync(int userId) {
			var dailyPerformances = _unitOfWork.GenericRepository<DailyPerformance>()
				.GetAll(filter: dp => dp.UserId == userId)
				.ToList();

			return await Task.Run(() => ConvertModelToViewModelList(dailyPerformances));
		}

		public async Task<DailyPerformanceViewModel> GetDailyPerformanceAsync(int userId, DateTime date) {
			var dailyPerformance = _unitOfWork.GenericRepository<DailyPerformance>()
				.GetAll(filter: dp => dp.UserId == userId && dp.Date == date.Date)
				.ToList().FirstOrDefault();
			if (dailyPerformance == null) {
				return null;
			}
			var vm = new DailyPerformanceViewModel(dailyPerformance);
			return vm;
		}

		public async Task<DailyPerformanceViewModel> GetTodaysDailyPerformance(int userId) {
			object[] KeyValues = new object[] { userId, DateTime.Now.Date };

			var dailyPerformance = await _unitOfWork.GenericRepository<DailyPerformance>()
				.GetUsingCompositeIdAsync(KeyValues);

			if (dailyPerformance == null)
				return null;

			var vm = new DailyPerformanceViewModel(dailyPerformance);
			return vm;
		}

		public async Task TrackDailyPerformanceAsync(DailyPerformanceViewModel dailyPerformance) {
			var currentDate = DateTime.UtcNow.Date; // Get today's date in UTC
			var model = new DailyPerformanceViewModel().ConvertViewModel(dailyPerformance);
			var ModelById = await _unitOfWork.GenericRepository<DailyPerformance>()
				.GetUsingCompositeIdAsync(new object[] { dailyPerformance.UserId, dailyPerformance.Date.Date });
			if (ModelById == null) {
				_unitOfWork.GenericRepository<DailyPerformance>().Add(model);
			} else {
				ModelById.Date = dailyPerformance.Date;
				ModelById.WordsLearnedCount = dailyPerformance.WordsLearnedCount;
				ModelById.WordsGuessedCount = dailyPerformance.WordsGuessedCount;
				ModelById.MinutesSpentLearning = dailyPerformance.MinutesSpentLearning;
				ModelById.User = dailyPerformance.User;
				ModelById.UserId = dailyPerformance.UserId;

				_unitOfWork.GenericRepository<DailyPerformance>().Update(ModelById);
			}
			await _unitOfWork.Save();
		}

		private async Task<List<DailyPerformanceViewModel>> ConvertModelToViewModelList(List<DailyPerformance> modelList) {
			return modelList.Select(x => new DailyPerformanceViewModel(x)).ToList();
		}
	}
}
