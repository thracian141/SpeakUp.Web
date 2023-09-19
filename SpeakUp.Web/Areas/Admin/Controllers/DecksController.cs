using hospitals.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpeakUp.Models;
using SpeakUp.Services;
using SpeakUp.Utilities;
using SpeakUp.ViewModels;
using System.Security.Claims;

namespace SpeakUp.Web.Areas.Admin.Controllers {
	[Area("Admin")]
	public class DecksController : Controller {
		private IDeckService _deck;
		private readonly UserManager<ApplicationUser> _userManager;
		private IWebHostEnvironment _env;
		private IWordService _word;
		private ISentenceService _sentence;
		private IGrammarService _grammar;


		public DecksController(IDeckService deck, UserManager<ApplicationUser> userManager,
			IWebHostEnvironment env, IWordService word, ISentenceService sentence,
			IGrammarService grammar) {
			_deck = deck;
			_userManager = userManager;
			_env = env;
			_word = word;
			_sentence = sentence;
			_grammar = grammar;
		}

		public async Task<IActionResult> Index() {
			var user = await _userManager.GetUserAsync(User);
			int? userId = null;
			if (user == null) {
				userId = 0;
			} else {
				userId = user.Id;
			}
			List<DeckViewModel> decks = await _deck.GetDecksByUserId((int)userId);
			ViewBag.CurrentUserId = userId;
			ViewBag.Decks = decks.Where(d => d.IsCourse == false);
			ViewBag.Courses = decks.Where(d => d.IsCourse == true);
			var ordered = decks.OrderBy(d => d.IsCourse).Reverse().ToList();
			return View(ordered);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id) {
			var viewModel = await _deck.GetDeckById(id);
			return View(viewModel);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(DeckViewModel vm) {
			if (vm.DeckImageFile != null) {
				ImageOperationsDecks image = new ImageOperationsDecks(_env);
				string filename = await image.ImageUpload(vm.DeckImageFile);
				vm.DeckImage = filename;
			}
			if (User.IsInRole(WebSiteRole.WebSite_Admin) || User.IsInRole(WebSiteRole.WebSite_Dev)) {
				if (vm.IsCourse) {
					var courseInstances = await _deck.GetCourseInstances(vm.DeckName);
				}
			}

			await _deck.UpdateDeck(vm);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public IActionResult Create() {
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(DeckViewModel vm) {
			int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			vm.UserId = userId;
			vm.User = await _userManager.FindByIdAsync(userId.ToString());

			ImageOperationsDecks image = new ImageOperationsDecks(_env);
			string filename = await image.ImageUpload(vm.DeckImageFile);
			vm.DeckImage = filename;

			await _deck.InsertDeck(vm);

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int id) {
			var grammars = await _grammar.GetGrammarsByDeckId(id);
			var words = await _word.GetWordsByDeckId(id);
			var sentences = await _sentence.GetSentencesByWordId(id);

			if (words != null) {
				foreach (var word in words) {
					await _word.DeleteWord(word.Id);
				}
			}
			if (grammars != null) {
				foreach (var grammar in grammars) {
					await _grammar.DeleteGrammar(grammar.Id);
				}
			}
			if (sentences != null) {
				foreach (var sentence in sentences) {
					await _sentence.DeleteSentence(sentence.Id);
				}
			}

			await _deck.DeleteDeck(id);
			return View("Index");
		}
		public async Task<IActionResult> NullLevels(int id) {
			var words = await _word.GetWordsByDeckId(id);
			foreach (var word in words) {
				word.Level = 0;
				await _word.UpdateWord(word);
			}

			var grammars = await _grammar.GetGrammarsByDeckId(id);
			foreach (var grammar in grammars) {
				grammar.Level = 0;
				await _grammar.UpdateGrammar(grammar);
			}

			var deck = await _deck.GetDeckById(id);
			deck.Level = 0;
			await _deck.UpdateDeck(deck);

			return RedirectToAction("Index");
		}
	}
}
