using SpeakUp.Utilities;
using SpeakUp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Services
{
	public interface IDeckService
	{
		Task<List<DeckViewModel>> GetAll();
		Task<DeckViewModel> GetDeckById(int DeckId);
        Task<DeckViewModel> GetDeckById(int? DeckId);
        Task UpdateDeck(DeckViewModel Room);
		Task InsertDeck(DeckViewModel Room);
		Task<int> InsertCopyDeck(DeckViewModel Deck);
		Task DeleteDeck(int id);
		Task<List<DeckViewModel>> GetDecksByUserId(int UserId);
		Task<List<DeckViewModel>> GetAllCourses();
		Task<List<DeckViewModel>> GetPersonalCourses(int userId);
		Task<List<DeckViewModel>> GetCourseInstances(string CourseName);
	}
}
