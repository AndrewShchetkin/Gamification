using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gamification.Models;

namespace Gamification.Data.Interfaces
{
    interface IUserAnswerRepository
    {
        Task<Guid> Create(UserAnswer newUserAnswer);
        Task<UserAnswer> GetUserAnswerById(Guid userAnswerId);
        Task<List<UserAnswer>> GetAllUserAnswersByUser(User user);
    }
}
