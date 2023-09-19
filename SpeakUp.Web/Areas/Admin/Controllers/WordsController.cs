using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpeakUp.Models;
using SpeakUp.Services;
using SpeakUp.ViewModels;

namespace SpeakUp.Web.Areas.Admin.Controllers {
	[Area("Admin")]
	public class WordsController : Controller {
		private IWordService _word;
		private IDeckService _deck;
		private IGrammarService _grammar;
		private ISentenceService _sentence;
		public WordsController(IWordService word, IDeckService deck,
			IGrammarService grammar, ISentenceService sentence) {
			_word = word;
			_deck = deck;
			_grammar = grammar;
			_sentence = sentence;
		}

		public IActionResult Index(int pageNumber = 1, int pageSize = 10) {
			return View(_word.GetAll(pageNumber, pageSize));
		}

		public async Task<IActionResult> WordsForDeck(int deckId) {
			var deck = await _deck.GetDeckById(deckId);
			Console.WriteLine("CURRENT DECK: " + deck.DeckName + " WITH ID: " + deck.Id);
			ViewData["CurrentDeck"] = deck;
			var wordsForDeck = _word.GetWordsByDeckId(deckId);
			return View(wordsForDeck.Result);
		}
		public async Task<IActionResult> WordsForGrammar(int grammarId) {
			var grammar = await _grammar.GetGrammarById(grammarId);
			ViewData["CurrentGrammar"] = grammar;
			var wordsForDeck = _word.GetWordsByGrammarId(grammarId);
			return View(await wordsForDeck);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id) {
			var viewModel = await _word.GetWordById(id);
			return View(viewModel);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(WordViewModel vm) {
			await _word.UpdateWord(vm);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public async Task<IActionResult> CreateForDeck(int deckId) {
			var deck = await _deck.GetDeckById(deckId);
			ViewData["CurrentDeck"] = deck;
			var viewModel = new WordViewModel { DeckId = deckId };
			return View(viewModel);
		}
		[HttpPost]
		public async Task<IActionResult> CreateForDeck(WordViewModel vm) {
			vm.NextReviewDate = DateTime.Now.AddMinutes(20);
			if (vm.Difficulty < 0)
				vm.Difficulty = 0;
			else if (vm.Difficulty > 10)
				vm.Difficulty = 10;

			vm.NextReviewDate = DateTime.Now;
			vm.DateAdded = DateTime.Now;
			vm.Level = 0;

			await _word.InsertWord(vm);
			return RedirectToAction("WordsForDeck", new { deckId = vm.DeckId });
		}
		[HttpGet]
		public async Task<IActionResult> CreateForGrammar(int grammarId) {
			var grammar = await _grammar.GetGrammarById(grammarId);
			ViewData["CurrentGrammar"] = grammar;
			var deck = await _deck.GetDeckById(grammar.DeckId);
			ViewData["CurrentDeck"] = deck;
			var viewModel = new WordViewModel { GrammarId = grammarId, DeckId = deck.Id };
			return View(viewModel);
		}
		[HttpPost]
		public async Task<IActionResult> CreateForGrammar(WordViewModel vm) {
			vm.NextReviewDate = DateTime.Now.AddMinutes(20);
			if (vm.Difficulty < 0)
				vm.Difficulty = 0;
			else if (vm.Difficulty > 10)
				vm.Difficulty = 10;

			vm.NextReviewDate = DateTime.Now;
			vm.DateAdded = DateTime.Now;
			vm.Level = 0;
			int grammarId = (int)vm.GrammarId;
			var grammar = await _grammar.GetGrammarById(grammarId);
			vm.DeckId = grammar.DeckId;

			await _word.InsertWord(vm);
			return RedirectToAction("WordsForDeck", new { grammarId = vm.GrammarId });
		}

		public async Task<IActionResult> Delete(int id) {
			await _word.DeleteWord(id);
			return View("Index");
		}

		[HttpGet]
		public async Task<IActionResult> MassCreate(int grammarId) {
			return View(await _grammar.GetGrammarById(grammarId));
		}


		[HttpPost]
		public async Task<IActionResult> MassCreate(IFormFile file, int deckId, int grammarId) {
			if (file != null) {
				using var reader = new StreamReader(file.OpenReadStream());
				int currentWordId = 0; // Initialize the currentWordId
				bool isWordLine = true; // Flag to track whether it's a word line or sentence line

				while (!reader.EndOfStream) {
					var line = reader.ReadLine().Trim();

					if (string.IsNullOrWhiteSpace(line)) {
						// Empty line indicates a new word-sentence group
						currentWordId = 0; // Reset the currentWordId
						isWordLine = true; // Reset the flag
						continue;
					}

					if (isWordLine) {
						// This is the word line
						var parts = line.Split(new[] { " | " }, StringSplitOptions.None);

						if (parts.Length == 2) {
							var wordViewModel = new WordViewModel {
								Level = 0,
								Difficulty = 0,
								DateAdded = DateTime.Now,
								LastReviewDate = DateTime.Now,
								NextReviewDate = DateTime.Now,
								Description = " ",
								DeckId = deckId,
								GrammarId = grammarId,
								Back = parts[0].Trim(), // Extract Back
								Front = parts[1].Trim() // Extract Front
							};

							currentWordId = await _word.InsertWordAndGetId(wordViewModel);
						}

						isWordLine = false; // Switch to sentence lines for the next iteration
					} else {
						// This is a sentence line
						if (currentWordId != 0) {
							var sentenceViewModel = new SentenceViewModel {
								Front = line.Split(new[] { " | " }, StringSplitOptions.None)[1].Trim(), // Extract Front
								Back = line.Split(new[] { " | " }, StringSplitOptions.None)[0].Trim(), // Extract Back
								WordId = currentWordId // Set the WordId to the current word's ID
							};

							await _sentence.AddSentence(sentenceViewModel);
						}
					}
				}
			}

			return RedirectToAction("Index");
		}

	}
}
