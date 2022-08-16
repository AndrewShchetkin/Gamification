using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamification.Data.Interfaces;
using Gamification.Models;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Data
{
    public class QuestionRepository:IQuestionRepository
    {
        private readonly ApplicationContext db;
        public QuestionRepository(ApplicationContext context)
        {
            db = context;
        }
        public async Task<Guid> Create(Question newQuestion)
        {
            db.Qusestions.Add(newQuestion);
            await db.SaveChangesAsync();
            return newQuestion.QuestionId;
        }
        public async Task<Question> GetQuestionById(Guid questionId)
        {
            return await db.Qusestions.FirstOrDefaultAsync(q=> q.QuestionId == questionId);
        }
        public async Task<List<Question>> GetAllQuestionsByQuiz(Quiz quiz)
        {
            var questions = await (from q in db.Qusestions where q.Quiz == quiz select q).ToListAsync();
            return questions;
        }
    }
}
