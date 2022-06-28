using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamification.Data.Interfaces;
using Gamification.Models;
using Microsoft.EntityFrameworkCore;


namespace Gamification.Data
{
    public class UserAnswerRepository:IUserAnswerRepository
    {
        private readonly ApplicationContext db;
        public UserAnswerRepository(ApplicationContext context)
        {
            db = context;
        }
        public async Task<Guid> Create(UserAnswer newUserAnswer)
        {
            db.UserAnswers.Add(newUserAnswer);
            await db.SaveChangesAsync();
            return newUserAnswer.UserAnswerId;
        }
        public async Task<UserAnswer> GetUserAnswerById(Guid userAnswerId)
        {
            return await db.UserAnswers.FirstOrDefaultAsync(ua => ua.UserAnswerId == userAnswerId);
        }
        public async Task<List<UserAnswer>> GetAllUserAnswersByUser(User user)
        {
            var userAnswers = await (from ua in db.UserAnswers where ua.User == user select ua).ToListAsync();
            return userAnswers;
        }
    }
}
