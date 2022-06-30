using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gamification.Models;

namespace Gamification.Data.Interfaces
{
    public interface IQuizRepository
    {
        Task<Guid> Create(Quiz newQuiz);

        Task<Guid> Remove(Quiz quiztoDelete);
        Task<Quiz> GetQuizById(Guid quizId);
        Task<Quiz> GetQuizByName(string quizName);
        Task<List<Quiz>> GetAllQuizzes();
    }
}
