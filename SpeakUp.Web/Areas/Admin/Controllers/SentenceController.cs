using Microsoft.AspNetCore.Mvc;
using SpeakUp.Models;
using SpeakUp.Services;
using SpeakUp.ViewModels;

namespace SpeakUp.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class SentenceController : Controller
	{
		private ISentenceService _sentence;
		private IWordService _word;
		private IGrammarService _grammar;
		public SentenceController(ISentenceService sentence, IWordService word, IGrammarService grammar)
		{
			_sentence = sentence;
			_word = word;
			_grammar = grammar;
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var viewModel = await _sentence.GetSentenceById(id);
			return View(viewModel);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(SentenceViewModel vm)
		{
			var word = await _word.GetWordById(vm.WordId);
			
			await _sentence.UpdateSentence(vm);
			return RedirectToAction("Index", new { id = word.GrammarId});
		}

		public async Task<IActionResult> Index(int id)
		{
			var grammar = await _grammar.GetGrammarById(id);
			ViewData["CurrentGrammar"] = grammar;

            var grammarWords = await _word.GetWordsByGrammarId(id);
            List<SentenceViewModel> sentences = new List<SentenceViewModel>();

            foreach (var word in grammarWords) {
                var wordSentences = await _sentence.GetSentencesByWordId(word.Id);
                sentences.AddRange(wordSentences);
            }

            return View(sentences);
        }

		[HttpGet]
		public async Task<IActionResult> Create(int wordId)
		{
			var word = await _word.GetWordById(wordId);
			ViewData["CurrentWord"] = word;
			var viewModel = new SentenceViewModel { WordId = wordId};
			return View(viewModel);
		}
		[HttpPost]
		public async Task<IActionResult> Create(SentenceViewModel vm)
		{
            var word = await _word.GetWordById(vm.WordId);

            await _sentence.AddSentence(vm);
			return RedirectToAction("Index", new { id = word.GrammarId });
		}

		[HttpGet]
		public async Task<IActionResult> MassCreate(int wordId)
		{
			return View(await _word.GetWordById(wordId));
		}
		[HttpPost]
		public async Task<IActionResult> MassCreate(IFormFile file, int wordId)
		{
			if (file != null)
			{
				using var reader = new StreamReader(file.OpenReadStream());
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					var parts = line.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
					if (parts.Length == 2)
					{
						string front = parts[1];
						string back = parts[0];

						var vm = new SentenceViewModel
						{
							Id = 0,
							Front = front,
							Back = back,
							WordId = wordId
						};
						await _sentence.AddSentence(vm);
					}
				}
			}

            var word = await _word.GetWordById(wordId);
            return RedirectToAction("Index", new { id =  word.GrammarId});
		}

		public async Task<IActionResult> Delete(int id) {
			var sent = await _sentence.GetSentenceById(id);
			var word = await _word.GetWordById(sent.WordId);
			await _sentence.DeleteSentence(id);
			return RedirectToAction("Index", new { id = word.GrammarId });
		}
	}
}
