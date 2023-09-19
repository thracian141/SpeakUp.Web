using SpeakUp.Utilities;
using SpeakUp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Services
{
	public interface IApplicationUserService
	{
		PagedResult<ApplicationUserViewModel> GetAll(int PageNumber, int PageSize);
		ApplicationUserViewModel GetById(int id);
	}
}
