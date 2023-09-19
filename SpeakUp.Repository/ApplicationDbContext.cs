using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpeakUp.Models;
using System.Reflection.Emit;

namespace SpeakUp.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Words> Words { get; set; }
        public DbSet<DailyPerformance> DailyPerformances { get; set; }
        public DbSet<Grammar> Grammars { get; set; }
        public DbSet<Sentence> Sentences { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            /// Set Primary Keys
            builder.Entity<ApplicationUser>().HasKey(e => e.Id);
            builder.Entity<Deck>().HasKey(e => e.Id);
            builder.Entity<Words>().HasKey(e => e.Id);
			builder.Entity<Grammar>().HasKey(e => e.Id);
			builder.Entity<DailyPerformance>().HasKey(dp => new { dp.UserId, dp.Date });
            builder.Entity<Sentence>().HasKey(e => e.Id);

            // Foreign Keys
            builder.Entity<ApplicationUser>()
                .HasMany(r => r.Decks)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
			builder.Entity<ApplicationUser>()
				.HasMany(r => r.DailyPerformances)
				.WithOne(e => e.User)
				.HasForeignKey(e => e.UserId);
			builder.Entity<Deck>()
                .HasMany(r => r.Words)
                .WithOne(e => e.Deck)
                .HasForeignKey(e => e.DeckId);
            builder.Entity<Deck>()
                .HasMany(r => r.Grammars)
                .WithOne(e => e.Deck)
                .HasForeignKey(e => e.DeckId);
            builder.Entity<Grammar>()
                .HasMany(r => r.Words)
                .WithOne(e => e.Grammar)
                .HasForeignKey(e => e.GrammarId);
			builder.Entity<Words>()
				.HasMany(r => r.Sentences)
				.WithOne(e => e.Word)
				.HasForeignKey(e => e.WordId);

			//Constraints
			builder.Entity<Grammar>().Property(e => e.Id).IsRequired();
			builder.Entity<Grammar>().Property(e => e.Name).IsRequired();
			builder.Entity<Grammar>().Property(e => e.DeckId).IsRequired();
			builder.Entity<Grammar>().Property(e => e.Level).IsRequired();
            builder.Entity<Sentence>().Property(e => e.Id).IsRequired();
			builder.Entity<Sentence>().Property(e => e.Front).IsRequired();
			builder.Entity<Sentence>().Property(e => e.Back).IsRequired();
			builder.Entity<Sentence>().Property(e => e.WordId).IsRequired();
			builder.Entity<DailyPerformance>().Property(e => e.Date).IsRequired();
			builder.Entity<DailyPerformance>().Property(e => e.WordsGuessedCount).IsRequired();
			builder.Entity<DailyPerformance>().Property(e => e.WordsLearnedCount).IsRequired();
			builder.Entity<DailyPerformance>().Property(e => e.MinutesSpentLearning).IsRequired();
			builder.Entity<DailyPerformance>().Property(e => e.WordsGuessedCount).HasDefaultValue(0);
			builder.Entity<DailyPerformance>().Property(e => e.WordsLearnedCount).HasDefaultValue(0);
			builder.Entity<DailyPerformance>().Property(e => e.MinutesSpentLearning).HasDefaultValue(0);
            builder.Entity<ApplicationUser>().Property(e => e.LastDeck).IsRequired(false);
			builder.Entity<ApplicationUser>().Property(e => e.UserName).IsRequired();
            builder.Entity<ApplicationUser>().Property(e => e.AccountCreatedDate).IsRequired();
            builder.Entity<Deck>().Property(e => e.DeckName).IsRequired();
            builder.Entity<Words>().Property(e => e.Front).IsRequired();
            builder.Entity<Words>().Property(e => e.Back).IsRequired();
            builder.Entity<Words>().Property(e => e.Level).IsRequired();
			builder.Entity<Words>().Property(e => e.Difficulty).IsRequired();
			builder.Entity<Words>().Property(e => e.DateAdded).IsRequired();
            builder.Entity<Words>().Property(e => e.LastReviewDate).IsRequired();
			builder.Entity<Words>().Property(e => e.NextReviewDate).IsRequired();
			builder.Entity<Deck>().Property(d => d.IsCourse).HasDefaultValue(false);
			builder.Entity<Deck>().Property(e => e.Level).IsRequired();
			builder.Entity<Words>().Property(e => e.Level).HasDefaultValue(0);
			builder.Entity<Deck>().Property(e => e.Level).HasDefaultValue(0);
			builder.Entity<Grammar>().Property(e => e.Level).HasDefaultValue(0);
			builder.Entity<Words>().Property(e => e.NextReviewDate).HasDefaultValue(DateTime.MinValue);

			//Default Decks

			//rest of
			base.OnModelCreating(builder);
        }
    }
}