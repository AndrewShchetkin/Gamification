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

            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminPassword = "123456";

            // добавляем роли
            Role adminRole = new Role { Id= Guid.NewGuid(),  Name = adminRoleName };
            Role userRole = new Role { Id = Guid.NewGuid(), Name = userRoleName };
            User adminUser = new User { Id = Guid.NewGuid(), UserName = "admin", Password = BCrypt.Net.BCrypt.HashPassword(adminPassword), RoleId = adminRole.Id }; // тут Role = userRole

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

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
