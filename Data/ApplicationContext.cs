using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quiz>()
                .HasAlternateKey(quiz => quiz.QuizName);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Team> Teams { get; set; } 
        public DbSet<Cell> Cells { get; set; }

        public DbSet<CommonMessage> CommonMessages { get; set; }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<Question> Qusestions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<TeamAnswer> TeamAnswers { get; set; }
    }
}
