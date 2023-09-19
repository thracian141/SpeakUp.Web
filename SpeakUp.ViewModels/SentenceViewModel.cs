using SpeakUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SpeakUp.ViewModels
{
	public class SentenceViewModel
	{
		public int Id { get; set; }
		public string Front { get; set; }
		public string Back { get; set; }
		public int WordId { get; set; }
		public Words? Word { get; set; }

		public SentenceViewModel()
		{

		}

		public SentenceViewModel(Sentence sentence)
		{
			Id = sentence.Id;
			Front = sentence.Front;
			Back = sentence.Back;
			WordId = sentence.WordId;
			Word = sentence.Word;
		}
		public Sentence ConvertViewModel(SentenceViewModel model)
		{
			return new Sentence
			{
				Id = model.Id,
				Front = model.Front,
				Back = model.Back,
				WordId = model.WordId,
				Word = model.Word
			};
		}
	}
}
