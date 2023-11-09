using hospitals.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpeakUp.Models;
using SpeakUp.Services;
using SpeakUp.ViewModels;
using SpeakUp.Web.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace SpeakUp.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private IDeckService _deck;
		private readonly UserManager<ApplicationUser> _userManager;
		private IWebHostEnvironment _env;
		private IWordService _word;
		private IGrammarService _grammar;
		private IDailyPerformanceService _dailyPerformance;

		public HomeController(ILogger<HomeController> logger, IDeckService deck,
			UserManager<ApplicationUser> userManager,
			IWebHostEnvironment env, IWordService wordService,
			IGrammarService grammarService, IDailyPerformanceService dailyPerformance) {
			_logger = logger;
			_deck = deck;
			_userManager = userManager;
			_env = env;
			_word = wordService;
			_grammar = grammarService;
			_dailyPerformance = dailyPerformance;
		}

		[HttpGet]
		public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
		{
			DeckViewModel? currentCourse = null;
			var user = await _userManager.GetUserAsync(User);
			int? userId = null;
			int? wordCount = 0;
			DailyPerformanceViewModel? currentDailyPerformance = null;
			List<DailyPerformanceViewModel>? allDailyPerformances = null;
			if (user == null)
			{
				userId = 0;
			}
			else
			{
				userId = user.Id;
				currentCourse = await _deck.GetDeckById(user.LastDeck);
				await _dailyPerformance.Create(user);
				if (userId != null && userId > 0) {
					currentDailyPerformance = await _dailyPerformance.GetDailyPerformanceAsync((int)userId, DateTime.Now.Date);
					allDailyPerformances = await _dailyPerformance.GetAllDailyPerformancesAsync((int)userId);
					wordCount = await _word.GetWordsByDeckCount(currentCourse.Id);
				}
			}

			ViewBag.CurrentUserId = userId;
			ViewBag.CurrentCourse = currentCourse;
			ViewBag.UserDecks = await _deck.GetDecksByUserId((int)userId);
			ViewBag.CurrentDailyPerformance = currentDailyPerformance;
			ViewBag.AllDailyPerformances = allDailyPerformances;
			ViewBag.DeckWordCount = wordCount;

			return View(await _deck.GetDecksByUserId((int)userId));
		}

		[HttpGet]
		public async Task<IActionResult> ChangeCourse()
		{
			var user = await _userManager.GetUserAsync(User);
			return View(await _deck.GetPersonalCourses(user.Id));
		}

		[HttpPost]
		public async Task<IActionResult> ChangeCourse(int id)
		{
			var deck = await _deck.GetDeckById(id);
			var user = await _userManager.GetUserAsync(User);
			user.LastDeck = deck.Id;
			await _userManager.UpdateAsync(user);

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> OfficialCourses()
		{
			return View(await _deck.GetAllCourses());
		}

		[HttpGet]
		public async Task<IActionResult> AddCourse(int id)
		{
			var originalDeck = await _deck.GetDeckById(id);
			if (originalDeck == null)
			{
				return NotFound();
			}

			var user = await _userManager.GetUserAsync(User);
			var copiedDeck = new DeckViewModel
			{
				DeckName = originalDeck.DeckName,
				DeckImage = originalDeck.DeckImage,
				DeckDescription = originalDeck.DeckDescription,
				UserId = user.Id,
				User = user,
				IsCourse = originalDeck.IsCourse,
				Level = 0
			};

			ViewBag.Course = copiedDeck;
			ViewBag.OriginalDeck = originalDeck;

			return View(copiedDeck);
		}
		[HttpPost]
		public async Task<IActionResult> AddCourse(DeckViewModel vm)
		{
			int originalDeckId = int.Parse(Request.Form["originalDeckId"]);
			var originalDeckWords = await _word.GetWordsByDeckId(originalDeckId);
			var originalDeckGrammars = await _grammar.GetGrammarsByDeckId(originalDeckId);

			var newDeckId = await _deck.InsertCopyDeck(vm);

			if (originalDeckWords != null)
			{
				foreach (var word in originalDeckWords)
				{
					word.DeckId = vm.Id;
					var copiedWord = new WordViewModel
					{
						Front = word.Front,
						Back = word.Back,
						Level = 0,
						Difficulty = word.Difficulty,
						DateAdded = DateTime.Now,
						LastReviewDate = DateTime.Now,
						NextReviewDate = DateTime.Now,
						Description = word.Description,

						Deck = word.Deck,
						DeckId = vm.Id
					};

					await _word.InsertWord(copiedWord);
				} //Copy Deck Words
			}
			if (originalDeckGrammars != null)
			{
				foreach (var gram in originalDeckGrammars)
				{
					gram.DeckId = vm.Id;
					var copiedGrammar = new GrammarViewModel
					{
						GrammarUrl = gram.GrammarUrl,
						Name = gram.Name,
						Level = 0,
						LastReviewDate = DateTime.Now,
						NextReviewDate = DateTime.Now,

						Deck = gram.Deck,
						DeckId = vm.Id
					};

					await _grammar.AddGrammar(copiedGrammar);
				} //Copy Deck Grammars
			}

			var user = await _userManager.GetUserAsync(User);
			user.LastDeck = newDeckId;
			Console.WriteLine("VIEW MODEL ID: " + vm.Id);
			Console.WriteLine("USER LAST DECK ID: " + user.LastDeck);
			await _userManager.UpdateAsync(user);

			return RedirectToAction("Index");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}