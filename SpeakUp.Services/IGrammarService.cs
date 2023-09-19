using SpeakUp.Models;
using SpeakUp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Services
{
	public interface IGrammarService
	{
		Task<List<GrammarViewModel>> GetAll();
		Task AddGrammar(GrammarViewModel Grammar);
		Task DeleteGrammar(int id);
		Task<GrammarViewModel> GetGrammarById(int GrammarId);
		Task UpdateGrammar(GrammarViewModel Grammar);
		Task<List<GrammarViewModel>> GetGrammarsByDeckId(int DeckId);

	}
}
