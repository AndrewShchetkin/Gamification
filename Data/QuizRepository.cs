using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamification.Data.Interfaces;
using Gamification.Models;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Data
{
    public class QuizRepository:IQuizRepository
    {
        private readonly ApplicationContext db;
        public QuizRepository(ApplicationContext context)
        {
            db = context;
        }
        public async Task<Guid> Create(Quiz newQuiz)
        {
            db.Quizzes.Add(newQuiz);
            await db.SaveChangesAsync();
            return newQuiz.QuizId;
        }
        public async Task<List<Quiz>> GetAllQuizzes()
        {
            var quizzes = await db.Quizzes.ToListAsync();
            return quizzes;
        }
        public async Task<Quiz> GetQuizById(Guid quizId)
        {
            return await db.Quizzes.FirstOrDefaultAsync(q => q.QuizId == quizId);
        }
        public async Task<Quiz> GetQuizByName(string quizName)
        {
            return await db.Quizzes.FirstOrDefaultAsync(q => q.QuizName == quizName);
        }
    }
}
