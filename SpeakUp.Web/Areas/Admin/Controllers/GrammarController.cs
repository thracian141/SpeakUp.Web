using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using SpeakUp.Models;
using SpeakUp.Services;
using SpeakUp.ViewModels;

namespace SpeakUp.Web.Areas.Admin.Controllers {
	[Area("Admin")]
	public class GrammarController : Controller {
		private IDeckService _deck;
		private IGrammarService _grammar;
		private IWordService _word;
		private ISentenceService _sentence;
		public GrammarController(IDeckService deck, IGrammarService grammar, IWordService word, ISentenceService sentence) {
			_deck = deck;
			_grammar = grammar;
			_word = word;
			_sentence = sentence;
		}

		public async Task<IActionResult> Index() {
			return View();
		}

		public async Task<IActionResult> GrammarsForDeck(int deckId) {
			var deck = await _deck.GetDeckById(deckId);
			ViewData["CurrentDeck"] = deck;
			var grammarsForDeck = _grammar.GetGrammarsByDeckId(deckId);

			return View(await grammarsForDeck);
		}

		[HttpGet]
		public async Task<IActionResult> Create(int deckId) {
			var deck = await _deck.GetDeckById(deckId);
			ViewData["CurrentDeck"] = deck;
			var viewModel = new GrammarViewModel { DeckId = deckId };
			return View(viewModel);
		}
		[HttpPost]
		public async Task<IActionResult> Create(GrammarViewModel vm) {
			vm.Level = 0;

			await _grammar.AddGrammar(vm);
			return RedirectToAction("GrammarsForDeck", new { deckId = vm.DeckId });
		}
		public async Task<IActionResult> Delete(int id) {
			var grammarwords = await _word.GetWordsByGrammarId(id);

			if (grammarwords != null) {
				foreach (var word in grammarwords) {
					await _word.DeleteWord(word.Id);
				}
			}

			await _deck.DeleteDeck(id);
			return View("Index");
		}
	}
}
