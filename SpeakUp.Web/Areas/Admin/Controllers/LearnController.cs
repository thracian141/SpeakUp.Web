using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpeakUp.Models;
using SpeakUp.Services;
using SpeakUp.ViewModels;
using System.Text.RegularExpressions;

namespace SpeakUp.Web.Areas.Admin.Controllers {
	[Area("Admin")]
	public class LearnController : Controller {
		private IApplicationUserService _userService;
		private IWordService _word;
		private IDeckService _deck;
		private IDailyPerformanceService _dailyPerformance;
		private ISentenceService _sentence;
		private readonly UserManager<ApplicationUser> _userManager;

		public LearnController(IWordService word,
		IDeckService deck, IApplicationUserService user,
		IDailyPerformanceService dailyPerformance, UserManager<ApplicationUser> userManager
		, ISentenceService sentence) {
			_userService = user;
			_word = word;
			_deck = deck;
			_dailyPerformance = dailyPerformance;
			_userManager = userManager;
			_sentence = sentence;
		}

		[HttpGet]
		public async Task<IActionResult> Learn(int? currentDeckId) {
			DeckViewModel currentDeck;
			var user = await _userManager.GetUserAsync(User);
			if (!currentDeckId.HasValue)
				currentDeck = _deck.GetDecksByUserId(user.Id).Result.FirstOrDefault(deck => _word.GetWordsByDeckId(deck.Id).Result.Any());
			else
				currentDeck = await _deck.GetDeckById((int)currentDeckId);

			if (currentDeck == null)
				return RedirectToAction("NoWordsFound", new { isCourse = (bool?)null });

			var decksForDropdown = _deck.GetDecksByUserId(user.Id).Result.Where(deck => _word.GetWordsByDeckId(deck.Id).Result.Any()).ToList();

			ViewBag.Decks = decksForDropdown;
			ViewBag.CurrentDeck = currentDeck;

			var dueWords = _word.GetDueWords().OrderBy(x => x.Difficulty).Where(x => x.DeckId == currentDeck?.Id).ToList();
			var CurrentWord = dueWords.FirstOrDefault();

			// Sentence retrieval
			SentenceViewModel? sentenceVm = await _sentence.GetSentenceForCurrentWord(CurrentWord);
			if (sentenceVm != null) {
				string back = sentenceVm.Back;
				string searchTerm = CurrentWord.Back;

				string pattern = $@"(.*?)(\b{Regex.Escape(searchTerm)}\b)(.*)";
				Match match = Regex.Match(back, pattern, RegexOptions.IgnoreCase);

				if (match.Success) {
					string leftPart = match.Groups[1].Value;
					string middlePart = match.Groups[2].Value;
					string rightPart = match.Groups[3].Value;

					ViewBag.FrontSentence = sentenceVm.Front;
					ViewBag.BackSentence = new string[] { leftPart, rightPart };
				} else {
					// Handle the case when searchTerm is not found
					ViewBag.FrontSentence = sentenceVm.Front;
					ViewBag.BackSentence = new string[] { back, "" };
				}
			}

			if (CurrentWord != null)
				return View(CurrentWord);
			else
				return RedirectToAction("NoWordsFound", new { isCourse = currentDeck.IsCourse });
		}

		[HttpPost]
		public async Task<IActionResult> Learn(int id, string userInput) {
			WordViewModel vm = await _word.GetWordById(id);
			bool isCorrect = userInput.Trim().ToLower() == vm.Back.Trim().ToLower();
			int change = 0;
			vm.LastReviewDate = DateTime.Now;

			var user = await _userManager.GetUserAsync(User);
			DailyPerformanceViewModel dailyPerformance = await _dailyPerformance.GetTodaysDailyPerformance(user.Id);
			if (isCorrect) {
				dailyPerformance.WordsGuessedCount += 1;
				if (vm.Level == 0) {
					dailyPerformance.WordsLearnedCount += 1;
				}
			}
			await _dailyPerformance.TrackDailyPerformanceAsync(dailyPerformance);

			{ // Difficulty Block
				if (vm.Difficulty > 8)
					change += 7;
				else if (vm.Difficulty > 5)
					change += 8;
				else if (vm.Difficulty > 3)
					change += 9;
				else if (vm.Difficulty >= 0)
					change += 10;
			}// Difficulty Block
			if (!isCorrect)
				change -= (change * 2) / 2;

			vm.Level += change;
			{
				if (vm.Level > 95) {
					vm.NextReviewDate = vm.LastReviewDate.AddMonths(2);
				} else if (vm.Level > 90) {
					vm.NextReviewDate = vm.LastReviewDate.AddMonths(1);
				} else if (vm.Level > 80) {
					vm.NextReviewDate = vm.LastReviewDate.AddDays(12);
				} else if (vm.Level > 70) {
					vm.NextReviewDate = vm.LastReviewDate.AddDays(6);
				} else if (vm.Level > 60) {
					vm.NextReviewDate = vm.LastReviewDate.AddDays(3);
				} else if (vm.Level > 50) {
					vm.NextReviewDate = vm.LastReviewDate.AddDays(1);
				} else if (vm.Level > 40) {
					vm.NextReviewDate = vm.LastReviewDate.AddHours(16);
				} else if (vm.Level > 30) {
					vm.NextReviewDate = vm.LastReviewDate.AddHours(6);
				} else if (vm.Level > 20) {
					vm.NextReviewDate = vm.LastReviewDate.AddHours(1);
				} else if (vm.Level > 10) {
					vm.NextReviewDate = vm.LastReviewDate.AddMinutes(30);
				} else if (vm.Level > 0) {
					vm.NextReviewDate = vm.LastReviewDate.AddMinutes(15);
				} else {
					vm.NextReviewDate = vm.LastReviewDate.AddMinutes(5);
				}
			} // Next Review Date Block

			if (vm.Level < 0)
				vm.Level = 0;
			if (vm.Level > 100)
				vm.Level = 100;

			await _word.UpdateWord(vm);

			return RedirectToAction("Learn", new { currentDeckId = vm.DeckId });
		}

		public IActionResult NoWordsFound(bool? isCourse) {
			ViewBag.IsCourse = (bool?)isCourse;
			return View();
		}
	}
}
