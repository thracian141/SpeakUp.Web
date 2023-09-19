using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Models
{
	public class Sentence
	{
		public int Id { get; set; }
		public string Front { get; set; }
		public string Back { get; set; }
		public int WordId { get; set; }
		public Words? Word { get; set; }
	}
}
