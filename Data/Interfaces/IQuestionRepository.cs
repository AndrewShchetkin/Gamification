using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gamification.Models;

namespace Gamification.Data.Interfaces
{
    public interface IQuestionRepository
    {
        Task<Guid> Create(Question newQuestion);
        Task<Question> GetQuestionById(Guid quizId);
        Task<List<Question>> GetAllQuestionsByQuiz(Quiz quiz);
    }
}
