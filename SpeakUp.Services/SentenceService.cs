using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SpeakUp.Models;
using SpeakUp.Repository;
using SpeakUp.Utilities;
using SpeakUp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpeakUp.Services
{
	public class SentenceService : ISentenceService
	{
		private IUnitOfWork _unitOfWork;

		public SentenceService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task AddSentence(SentenceViewModel sentenceVm)
		{
			var model = new SentenceViewModel().ConvertViewModel(sentenceVm);
			_unitOfWork.GenericRepository<Sentence>().Add(model);
			await _unitOfWork.Save();
		}

		public async Task DeleteSentence(int id)
		{
			var model = _unitOfWork.GenericRepository<Sentence>().GetById(id);
			_unitOfWork.GenericRepository<Sentence>().Delete(model);
			await _unitOfWork.Save();
		}


		public async Task UpdateSentence(SentenceViewModel sentenceVm)
		{
			var model = new SentenceViewModel().ConvertViewModel(sentenceVm);
			var ModelById = _unitOfWork.GenericRepository<Sentence>().GetById(model.Id);
			ModelById.Id = sentenceVm.Id;
			ModelById.Front = sentenceVm.Front;
			ModelById.Back = sentenceVm.Back;
			ModelById.WordId = sentenceVm.WordId;
			ModelById.Word = sentenceVm.Word;

			_unitOfWork.GenericRepository<Sentence>().Update(ModelById);
			await _unitOfWork.Save();
		}

		public async Task<List<SentenceViewModel>> GetAll()
		{
			var modelList = _unitOfWork.GenericRepository<Sentence>()
				.GetAll()
				.ToList();

			return await ConvertModelToViewModelList(modelList);
		}

		public async Task<SentenceViewModel> GetSentenceById(int sentenceId)
		{
			var model = await _unitOfWork.GenericRepository<Sentence>()
						  .GetByIdAsync(x => x.Id == sentenceId);
			var vm = new SentenceViewModel(model);
			return vm;
		}

		public async Task<List<SentenceViewModel>> GetSentencesByWordId(int WordId)
		{
			var modelList = _unitOfWork.GenericRepository<Sentence>()
				.GetAll()
				.Where(s => s.WordId == WordId)
				.ToList();

			return await ConvertModelToViewModelList(modelList);
		}

		public async Task<SentenceViewModel> GetSentenceForCurrentWord(WordViewModel wordVm)
		{
			Console.OutputEncoding = Encoding.UTF8;
			var list = _unitOfWork.GenericRepository<Sentence>().GetAll().ToList();
			Console.WriteLine("DEBUG LIST SENT: ");
			foreach (var item in list)
			{
				Console.WriteLine(item.Back);
			}
			var allSentences = await ConvertModelToViewModelList(list);
			var wordPattern = @"\b" + Regex.Escape(wordVm.Back) + @"\b"; // Word boundary regex pattern

			var matchingSentences = new List<SentenceViewModel>();

			foreach (var sentence in allSentences)
			{
				var sentenceWords = Regex.Matches(sentence.Back, @"[\p{L}-]+").Cast<Match>().Select(match => match.Value);

				foreach (var sentenceWord in sentenceWords)
				{
					if (Regex.IsMatch(sentenceWord, wordPattern, RegexOptions.IgnoreCase))
					{
						matchingSentences.Add(sentence);
						break; // No need to check further words in this sentence
					}
				}
			}

			if (matchingSentences.Count > 0) // Return a randomly selected matching sentence
			{
				Console.WriteLine("MATCHING SENTENCES COUNT: " + matchingSentences.Count);
				foreach (var x in matchingSentences)
				{
					Console.WriteLine(x.Front);
				}
				var random = new Random();
				var randomIndex = random.Next(0, matchingSentences.Count);
				return matchingSentences[randomIndex]; 
			}

			return null;
		}

		private async Task<List<SentenceViewModel>> ConvertModelToViewModelList(List<Sentence> modelList)
		{
			return await Task.Run(() => modelList.Select(x => new SentenceViewModel(x)).ToList());
		}
	}
}
