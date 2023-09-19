using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Models
{
	public class ApplicationUser : IdentityUser<int>
	{
		[Key]
		public override int Id { get; set; }
		public override string UserName { get; set; }
		public string? DisplayName { get; set; }
		public string? ProfilePictureUrl { get; set; }
		public DateTime AccountCreatedDate { get; set; }
		public ICollection<Deck>? Decks { get; set; }
		public ICollection<DailyPerformance>? DailyPerformances { get; set;}
		public int? LastDeck { get; set; }
	}
}
