using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
	public class DeckService : IDeckService
	{
		private IUnitOfWork _unitOfWork;

		public DeckService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task DeleteDeck(int id)
		{
			var model = _unitOfWork.GenericRepository<Deck>().GetById(id);
			_unitOfWork.GenericRepository<Deck>().Delete(model);
			await _unitOfWork.Save();
		}

		public async Task<List<DeckViewModel>> GetAllCourses()
		{
			var modelList = _unitOfWork.GenericRepository<Deck>()
			.GetAll()
			.Where(d => d.IsCourse == true && d.UserId == 3)
			.ToList();

			return await Task.Run(() => ConvertModelToViewModelList(modelList));
		}

		public async Task<List<DeckViewModel>> GetPersonalCourses(int userId)
		{
			var personalCourses = _unitOfWork.GenericRepository<Deck>()
			.GetAll()
			.Where(d => d.IsCourse == true && d.UserId == userId)
			.ToList();

			var restOfCourses = _unitOfWork.GenericRepository<Deck>()
			.GetAll()
			.Where(d => d.IsCourse == true && d.UserId == 3)
			.ToList();

			var totalCourses = new List<Deck>();

			foreach (var course in restOfCourses)
			{
				bool isOwned = false;
				foreach (var ownedCourse in personalCourses)
				{
					if (course.DeckName == ownedCourse.DeckName)
					{
						isOwned = true; break;
					}
				}
				if (!isOwned)
				{
					totalCourses.Add(course);
				}
			}
			totalCourses.AddRange(personalCourses);
			totalCourses.Reverse();


			return await Task.Run(() => ConvertModelToViewModelList(totalCourses));
		}

		public async Task<List<DeckViewModel>> GetAll()
		{
			var modelList = _unitOfWork.GenericRepository<Deck>()
				.GetAll()
				.ToList();

			return await Task.Run(() => ConvertModelToViewModelList(modelList));
		}

		public async Task<DeckViewModel> GetDeckById(int DeckId)
		{
			var model = _unitOfWork.GenericRepository<Deck>().GetById(DeckId);
			var vm = new DeckViewModel(model);
			return vm;
		}
        public async Task<DeckViewModel> GetDeckById(int? DeckId)
        {
			if (DeckId == null)
			{
				return null;
			}
			else
			{
                var model = _unitOfWork.GenericRepository<Deck>().GetById(DeckId);
                var vm = new DeckViewModel(model);
                return vm;
            }    
        }

        public async Task InsertDeck(DeckViewModel Deck)
		{
			Console.WriteLine("INSERT REACHED");
			var model = new DeckViewModel().ConvertViewModel(Deck);
			Console.WriteLine(model.DeckName + " BEING CREATED");
			_unitOfWork.GenericRepository<Deck>().Add(model);
			Console.WriteLine("GENERIC REPOSITORY CREATED");
			await _unitOfWork.Save();
		}

		public async Task<int> InsertCopyDeck(DeckViewModel Deck)
		{
			Console.WriteLine("INSERT REACHED");
			var model = new DeckViewModel().ConvertViewModel(Deck);
			Console.WriteLine(model.DeckName + " BEING CREATED");
			_unitOfWork.GenericRepository<Deck>().Add(model);
			Console.WriteLine("GENERIC REPOSITORY CREATED");
			await _unitOfWork.Save();

			Console.WriteLine("MODEL ID: " + model.Id);
			return model.Id;
		}
		public async Task UpdateDeck(DeckViewModel Deck)
		{
			var model = new DeckViewModel().ConvertViewModel(Deck);
			var ModelById = _unitOfWork.GenericRepository<Deck>().GetByIdAsync().Result;
			ModelById.Id = Deck.Id;
			ModelById.DeckName = Deck.DeckName;
			ModelById.DeckImage = Deck.DeckImage;
			ModelById.DeckDescription = Deck.DeckDescription;
			ModelById.UserId = Deck.UserId;
			ModelById.User = Deck.User;
			ModelById.IsCourse = Deck.IsCourse;
			ModelById.Grammars = Deck.Grammars;


			_unitOfWork.GenericRepository<Deck>().Update(ModelById);
			await _unitOfWork.Save();
		}

		private async Task<List<DeckViewModel>> ConvertModelToViewModelList(List<Deck> modelList)
		{
			return modelList.Select(x => new DeckViewModel(x)).ToList();
		}
		public async Task<List<DeckViewModel>> GetDecksByUserId(int UserId)
		{
			var modelList = _unitOfWork.GenericRepository<Deck>()
			.GetAll()
			.Where(d => d.UserId == UserId)
			.ToList();

			return await Task.Run(() => ConvertModelToViewModelList(modelList));
		}
		public async Task<List<DeckViewModel>> GetCourseInstances(string CourseName)
		{
			var modelList = _unitOfWork.GenericRepository<Deck>()
			.GetAll()
			.Where(d => d.DeckName == CourseName && d.IsCourse == true)
			.ToList();

			return await Task.Run(() => ConvertModelToViewModelList(modelList));
		}
	}
}
