using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gamification.Models;

namespace Gamification.Data.Interfaces
{
    interface IAnswerRepository
    {
        Task<Guid> Create(Answer newAnswer);
        Task<Answer> GetAnswerById(Guid answerId);
        Task<List<Answer>> GetAllAnswersByQuestion(Question question);
    }
}
