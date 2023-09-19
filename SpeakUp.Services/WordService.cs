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

namespace SpeakUp.Services {
	public class WordService : IWordService {
		private IUnitOfWork _unitOfWork;

		public WordService(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork;
		}

		public async Task DeleteWord(int id) {
			var model = _unitOfWork.GenericRepository<Words>().GetById(id);
			_unitOfWork.GenericRepository<Words>().Delete(model);
			await _unitOfWork.Save();
		}

		public async Task<PagedResult<WordViewModel>> GetAll(int pageNumber, int pageSize) {
			var vm = new WordViewModel();
			int totalCount;
			List<WordViewModel> vmList = new List<WordViewModel>();
			try {
				int ExcludeRecords = (pageNumber * pageSize) - pageSize;

				var modelList = _unitOfWork.GenericRepository<Words>().GetAll()
					.Skip(ExcludeRecords).Take(pageSize).ToList();

				totalCount = _unitOfWork.GenericRepository<Words>().GetAll().ToList().Count;

				vmList = await ConvertModelToViewModelList(modelList);
			} catch (Exception) {
				throw;
			}
			var result = new PagedResult<WordViewModel> {
				Data = vmList,
				TotalItems = totalCount,
				PageNumber = pageNumber,
				PageSize = pageSize
			};
			return result;
		}

		public async Task<WordViewModel> GetWordById(int WordId) {
			var model = await _unitOfWork.GenericRepository<Words>()
						  .GetByIdAsync(x => x.Id == WordId);
			var vm = new WordViewModel(model);
			return vm;
		}
		public async Task<List<WordViewModel>> GetWordsByDeckId(int DeckId) {
			var modelList = _unitOfWork.GenericRepository<Words>()
				.GetAll()
				.Where(w => w.DeckId == DeckId)
				.ToList();

			return await ConvertModelToViewModelList(modelList);
		}

		public async Task<int> GetWordsByDeckCount(int DeckId) {
			var wordCount = _unitOfWork.GenericRepository<Words>()
				.GetAll().Where(w => w.DeckId == DeckId)
				.ToList();

			Console.WriteLine(wordCount.Count + " IS THE COUNT AAAAAAAA");

			return wordCount.Count;
		}

		public async Task<List<WordViewModel>> GetWordsByGrammarId(int GrammarId) {
			var modelList = _unitOfWork.GenericRepository<Words>()
				.GetAll()
				.Where(w => w.GrammarId == GrammarId)
				.ToList();

			return await ConvertModelToViewModelList(modelList);
		}

		public async Task InsertWord(WordViewModel Word) {
			var model = new WordViewModel().ConvertViewModel(Word);
			_unitOfWork.GenericRepository<Words>().Add(model);
			await _unitOfWork.Save();
		}

		public async Task<int> InsertWordAndGetId(WordViewModel Word) {
			var model = new WordViewModel().ConvertViewModel(Word);
			_unitOfWork.GenericRepository<Words>().Add(model);
			await _unitOfWork.Save();

			Console.WriteLine("RETURNED WORD WITH ID: " + model.Id);
			return model.Id;
		}

		public async Task UpdateWord(WordViewModel Word) {
			var model = new WordViewModel().ConvertViewModel(Word);
			var ModelById = _unitOfWork.GenericRepository<Words>().GetById(model.Id);
			ModelById.Id = Word.Id;
			ModelById.Front = Word.Front;
			ModelById.Back = Word.Back;
			ModelById.Level = Word.Level;
			ModelById.Difficulty = Word.Difficulty;
			ModelById.DateAdded = Word.DateAdded;
			ModelById.LastReviewDate = Word.LastReviewDate;
			ModelById.NextReviewDate = Word.NextReviewDate;
			ModelById.Description = Word.Description;
			ModelById.DeckId = Word.DeckId;
			ModelById.Deck = Word.Deck;
			ModelById.GrammarId = Word.GrammarId;
			ModelById.Grammar = Word.Grammar;

			_unitOfWork.GenericRepository<Words>().Update(ModelById);
			await _unitOfWork.Save();
		}

		private async Task<List<WordViewModel>> ConvertModelToViewModelList(List<Words> modelList) {
			return await Task.Run(() => modelList.Select(x => new WordViewModel(x)).ToList());
		}

		public List<WordViewModel> GetDueWords() {
			var modelList = _unitOfWork.GenericRepository<Words>()
				.GetAll()
				.Where(w => w.NextReviewDate <= DateTime.Now)
				.ToList();

			Console.WriteLine("WORDS DUE: " + modelList.Count);

			return ConvertModelToViewModelList(modelList).Result;
		}
	}
}
