using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamification.Data.Interfaces;
using Gamification.Models;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Data
{
    public class AnswerRepository:IAnswerRepository
    {
        private readonly ApplicationContext db;
        public AnswerRepository(ApplicationContext context)
        {
            db = context;
        }
        public async Task<Guid> Create(Answer newAnswer)
        {
            db.Answers.Add(newAnswer);
            await db.SaveChangesAsync();
            return newAnswer.AnswerId;
        }
        public async Task<Answer> GetAnswerById(Guid answerId)
        {
            return await db.Answers.FirstOrDefaultAsync(a => a.AnswerId == answerId);
        }
        public async Task<List<Answer>> GetAllAnswersByQuestion(Question question)
        {
            var answers = await (from a in db.Answers where a.Question.QuestionId == question.QuestionId select a).ToListAsync();
            return answers;
        }
    }
}
