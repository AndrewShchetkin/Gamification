using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Gamification.Data.Interfaces;
using Gamification.Models;
using Gamification.Data;
using System.Threading;
using Gamification.Utilities.Timers;
using Gamification.Services;

namespace Gamification.Hubs
{
    public class QuizHub : Hub
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoundRepository _roundRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IUserAnswerRepository _userAnswerRepository;
        private readonly ITeamAnswerRepository _teamAnswerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly QuizService _quizService;
        public QuizHub(IRoundRepository roundRepository, 
            IQuizRepository quizRepository, 
            IUserRepository userRepository,
            IUserAnswerRepository userAnswerRepository,
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            ITeamAnswerRepository teamAnswerRepository,
            ITeamRepository teamRepository,
            QuizService quizService)
        {
            _roundRepository = roundRepository;
            _quizRepository = quizRepository;
            _userRepository = userRepository;
            _userAnswerRepository = userAnswerRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _teamAnswerRepository = teamAnswerRepository;
            _teamRepository = teamRepository;
            _quizService = quizService;
        }
        public async Task JoinTeamRound(string teamId) //добавление пользователя в группу его команды
        {
            Team team = await _teamRepository.GetTeamById(Guid.Parse(teamId));
            Round round = await _roundRepository.GetCurrentRoundByTeamName(team.TeamName);
            if (round == null)
            {
                Quiz quiz = (await _quizRepository.GetAllQuizzes())[0]; //Временный вариант
                await _roundRepository.Create(new Round { RoundId = Guid.NewGuid(), Quiz = quiz, StartTime = DateTime.UtcNow, Team = team});
            }
            
            await Groups.AddToGroupAsync(Context.ConnectionId, teamId);

            if (_quizService.teamTimers.ContainsKey(teamId)) //обновление клиентов в сервисе
            {
                _quizService.teamUsers[teamId] = Clients.Group(teamId);
            }
        }
        public async Task RoundStart(string teamId) //будет запускаться таймер
        {
            Team team = await _teamRepository.GetTeamById(Guid.Parse(teamId));
            Round round = await _roundRepository.GetCurrentRoundByTeamName(team.TeamName);
            Quiz quiz = round.Quiz;
            if (!_quizService.teamTimers.ContainsKey(teamId))
            {
                List<Question> questions = await _questionRepository.GetAllQuestionsByQuiz(quiz);
                List<List<Answer>> answers = new List<List<Answer>>();
                foreach (Question question in questions)
                {
                    answers.Add(await _answerRepository.GetAllAnswersByQuestion(question));
                }
                _quizService.InitializeRound(teamId, questions, answers, Clients.Group(teamId), 30000);
            }
        }
        public async static void NewQuestion(QuizEventArgs quizEventArgs) //отправка новых вопросов на фронт
        {
            IClientProxy hubClients = quizEventArgs.users;
            Question question = quizEventArgs.question;
            List<Answer> answers = quizEventArgs.answers;
            await hubClients.SendAsync("NewQuestion", new
            {
                text = question.QuestionText,
                answers = answers.Select((answer) => new
                {
                    id = answer.AnswerId,
                    text = answer.AnswerText,
                })
            });
        }
        public async Task NewUserAnswer(string userName, string answerId) //получение ответов пользователей
        {
            User user = _userRepository.GetUserByUserName(userName);
            Answer answer = await _answerRepository.GetAnswerById(Guid.Parse(answerId));
            await _userAnswerRepository.Create(new UserAnswer { UserAnswerId = Guid.NewGuid(), Answer = answer, User = user, AnswerDate = DateTime.UtcNow });
        }
        public async void TeamAnswer(Team team, Round round) //выбор ответа команды
        {
            //List<User> teamUsers = team.Users;
            //List<List<UserAnswer>> asnwersOfUsersInTeam = new List<List<UserAnswer>>();
            //foreach(User user in teamUsers)
            //{
            //    asnwersOfUsersInTeam.Add(await _userAnswerRepository.GetAllUserAnswersByUser(user));
            //}

            //List<Question> questions = await _questionRepository.GetAllQuestionsByQuiz(round.Quiz);
            //List<List<Answer>> answers = new List<List<Answer>>();
            //foreach(Question question in questions)
            //{
            //    answers.Add(await _answerRepository.GetAllAnswersByQuestion(question));
            //}
            
        }
        public async Task RoundOver(string teamId)//запись времени конца раунда
        {
            Team team = await _teamRepository.GetTeamById(Guid.Parse(teamId));
            Round round = await _roundRepository.GetCurrentRoundByTeamName(team.TeamName);
            
            if (round != null)
            {
                await _roundRepository.UpdataEndDate(round, DateTime.UtcNow);
            }
            TeamAnswer(team, round);
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
