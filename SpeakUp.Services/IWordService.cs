using SpeakUp.Utilities;
using SpeakUp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Services
{
	public interface IWordService
	{
		Task<PagedResult<WordViewModel>> GetAll(int pageNumber, int pageSize);
		Task<WordViewModel> GetWordById(int WordId);
		Task<List<WordViewModel>> GetWordsByDeckId(int DeckId);
		Task<int> GetWordsByDeckCount(int DeckId);
		Task<List<WordViewModel>> GetWordsByGrammarId(int DeckId);
		Task UpdateWord(WordViewModel wordVm);
		Task InsertWord(WordViewModel wordVm);
		Task<int> InsertWordAndGetId(WordViewModel Word);
		Task DeleteWord(int id);
		List<WordViewModel> GetDueWords();
	}
}
