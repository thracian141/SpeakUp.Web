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
	public class GrammarService : IGrammarService
	{
		private IUnitOfWork _unitOfWork;

		public GrammarService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<List<GrammarViewModel>> GetAll()
		{
			var modelList = _unitOfWork.GenericRepository<Grammar>().GetAll().ToList();

			return await ConvertModelToViewModelList(modelList);
		}

		public async Task AddGrammar(GrammarViewModel Grammar)
		{
			var model = new GrammarViewModel().ConvertViewModel(Grammar);
			_unitOfWork.GenericRepository<Grammar>().Add(model);
			await _unitOfWork.Save();
		}

		public async Task DeleteGrammar(int id)
		{
			var model = _unitOfWork.GenericRepository<Grammar>().GetById(id);
			_unitOfWork.GenericRepository<Grammar>().Delete(model);
			await _unitOfWork.Save();
		}

		public async Task<GrammarViewModel> GetGrammarById(int GrammarId)
		{
			var model = await _unitOfWork.GenericRepository<Grammar>()
										.GetByIdAsync(x => x.Id == GrammarId);
			var vm = new GrammarViewModel(model);
			return vm;
		}

		public async Task UpdateGrammar(GrammarViewModel Grammar)
		{
			var model = new GrammarViewModel().ConvertViewModel(Grammar);
			var ModelById = _unitOfWork.GenericRepository<Grammar>().GetById(model.Id);
			ModelById.Id = Grammar.Id;
			ModelById.Name = Grammar.Name;
			ModelById.LastReviewDate = Grammar.LastReviewDate;
			ModelById.NextReviewDate = Grammar.NextReviewDate;
			ModelById.GrammarUrl = Grammar.GrammarUrl;
			ModelById.DeckId = Grammar.DeckId;
			ModelById.Deck = Grammar.Deck;
			ModelById.Words = Grammar.Words;

			_unitOfWork.GenericRepository<Grammar>().Update(ModelById);
			await _unitOfWork.Save();
		}

		public async Task<List<GrammarViewModel>> GetGrammarsByDeckId(int DeckId)
		{
			var modelList = _unitOfWork.GenericRepository<Grammar>()
				.GetAll()
				.Where(w => w.DeckId == DeckId)
				.ToList();

			return await ConvertModelToViewModelList(modelList);
		}

		private async Task<List<GrammarViewModel>> ConvertModelToViewModelList(List<Grammar> modelList)
		{
			return await Task.Run(() => modelList.Select(x => new GrammarViewModel(x)).ToList());
		}
	}
}
