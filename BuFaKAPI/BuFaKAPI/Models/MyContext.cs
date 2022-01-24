using Microsoft.EntityFrameworkCore;
using BuFaKAPI.Models;
namespace BuFaKAPI.Models
{
    using Microsoft.EntityFrameworkCore;
    using WebApplication1.Models;

    public class MyContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conference_Application>().HasKey(i => new { i.ConferenceID, i.ApplicantUID });
            modelBuilder.Entity<Workshop_Application>().HasKey(i => new { i.WorkshopID, i.ApplicantUID });
            modelBuilder.Entity<Administrator>().HasKey(i => new { i.UID, i.ConferenceID });
        }

        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {
        }

        public DbSet<WebApplication1.Models.User> User { get; set; }

        public DbSet<WebApplication1.Models.Council> Council { get; set; }

        public DbSet<WebApplication1.Models.Conference> Conference { get; set; }

        public DbSet<WebApplication1.Models.Conference_Application> Conference_Application { get; set; }

        public DbSet<WebApplication1.Models.Workshop> Workshop { get; set; }

        public DbSet<WebApplication1.Models.Workshop_Application> Workshop_Application { get; set; }

        public DbSet<WebApplication1.Models.Auth> Auth { get; set; }

        public DbSet<BuFaKAPI.Models.Administrator> Administrator { get; set; }

        public DbSet<BuFaKAPI.Models.Sensible> Sensible { get; set; }

        public DbSet<BuFaKAPI.Models.History> History { get; set; }

        public DbSet<BuFaKAPI.Models.Newsletter> Newsletter { get; set; }

        public DbSet<BuFaKAPI.Models.ApplicationAuth> ApplicationAuth { get; set; }

        public DbSet<BuFaKAPI.Models.News> News { get; set; }

        public DbSet<BuFaKAPI.Models.Event> Event { get; set; }

        public DbSet<BuFaKAPI.Models.Place> Place { get; set; }

        public DbSet<BuFaKAPI.Models.VotingQuestion> VotingQuestion { get; set; }

        public DbSet<BuFaKAPI.Models.VotingAnswer> VotingAnswer { get; set; }

        public DbSet<BuFaKAPI.Models.VotingMajority> VotingMajority { get; set; }

        public DbSet<WebApplication1.Models.Travel> Travel { get; set; }
    }
}
