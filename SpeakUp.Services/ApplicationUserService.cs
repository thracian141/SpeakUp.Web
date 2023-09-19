using SpeakUp.Models;
using SpeakUp.Repository;
using SpeakUp.Utilities;
using SpeakUp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Services
{
	public class ApplicationUserService : IApplicationUserService
	{
		private IUnitOfWork _unitOfWork;
		public ApplicationUserService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public PagedResult<ApplicationUserViewModel> GetAll(int PageNumber, int PageSize)
		{
			var vm = new ApplicationUserViewModel();
			int totalCount;
			List<ApplicationUserViewModel> vmList = new List<ApplicationUserViewModel>();
			try
			{
				int ExcludeRecords = (PageSize * PageNumber) - PageSize;

				var modelList = _unitOfWork.GenericRepository<ApplicationUser>().GetAll()
					.Skip(ExcludeRecords).Take(PageSize).ToList();
				totalCount = _unitOfWork.GenericRepository<ApplicationUser>().GetAll().ToList().Count;

				vmList = ConvertViewModelToModel(modelList);
			}
			catch (Exception)
			{

				throw;
			}
			var result = new PagedResult<ApplicationUserViewModel>
			{
				Data = vmList,
				TotalItems = totalCount,
				PageNumber = PageNumber,
				PageSize = PageSize
			};
			return result;
		}

		public ApplicationUserViewModel GetById(int DeckId)
		{
			var model = _unitOfWork.GenericRepository<ApplicationUser>().GetById(DeckId);
			var vm = new ApplicationUserViewModel(model);
			return vm;
		}

		private List<ApplicationUserViewModel> ConvertViewModelToModel(List<ApplicationUser> modelList)
		{
			return modelList.Select(x => new ApplicationUserViewModel(x)).ToList();
		}
	}
}
