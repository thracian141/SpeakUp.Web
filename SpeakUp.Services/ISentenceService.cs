using SpeakUp.Utilities;
using SpeakUp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Services
{
	public interface ISentenceService
	{
		Task<List<SentenceViewModel>> GetAll();
		Task<SentenceViewModel> GetSentenceById(int sentenceId);
		Task<List<SentenceViewModel>> GetSentencesByWordId(int WordId);
		Task UpdateSentence(SentenceViewModel sentenceVm);
		Task AddSentence(SentenceViewModel sentenceVm);
		Task DeleteSentence(int id);
		Task<SentenceViewModel> GetSentenceForCurrentWord(WordViewModel wordVm);
	}
}
