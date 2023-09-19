using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Repository
{
	public interface IUnitOfWork
	{
		IGenericRepository<T> GenericRepository<T>() where T : class;
		Task Save();
	}
}
