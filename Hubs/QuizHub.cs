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
        public CustomTimer timer;
        public QuizHub(IRoundRepository roundRepository, 
            IQuizRepository quizRepository, 
            IUserRepository userRepository,
            IUserAnswerRepository userAnswerRepository,
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            ITeamAnswerRepository teamAnswerRepository,
            ITeamRepository teamRepository)
        {
            _roundRepository = roundRepository;
            _quizRepository = quizRepository;
            _userRepository = userRepository;
            _userAnswerRepository = userAnswerRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _teamAnswerRepository = teamAnswerRepository;
            _teamRepository = teamRepository;
        }
        public async Task JoinTeamRound(string teamId) //добавление пользователя в группу его команды
        {
            Team team = await _teamRepository.GetTeamById(Guid.Parse(teamId));
            Round round = await _roundRepository.GetCurrentRoundByTeamName(team.TeamName);
            
            if (round == null)
            {
                Quiz quiz = (await _quizRepository.GetAllQuizzes())[0]; //Временный вариант
                await _roundRepository.Create(new Round { RoundId = Guid.NewGuid(), Quiz = quiz, StartTime = DateTime.Now, Team = team});
            }
            
            await Groups.AddToGroupAsync(Context.ConnectionId, teamId);

            CustomTimer temp = CustomTimer.Timers.GetValueOrDefault(teamId);
            if (temp != null && temp.Enabled)
            {
                temp.hubCallerClients = Clients.Group(teamId);
            }
        }
        public async Task RoundStart(string teamId) //будет запускаться таймер
        {
            CustomTimer temp = CustomTimer.Timers.GetValueOrDefault(teamId, null);
            if(temp == null || !temp.Enabled)
            {
                timer = new CustomTimer(30000);
                timer.callerContext = Context;
                timer.team = teamId;
                timer.Elapsed += aTimer_Elapsed;
                timer.Interval = 30000;
                timer.questions = await _questionRepository.GetAllQuestionsByQuiz(_quizRepository.GetAllQuizzes().Result[0]);
                timer.answers = new List<List<Answer>>();
                timer.hubCallerClients = Clients.Group(teamId);
                foreach (Question q in timer.questions)
                {
                    timer.answers.Add(await _answerRepository.GetAllAnswersByQuestion(q));
                }
                timer.Enabled = true;
                timer = CustomTimer.Timers.GetOrAdd(teamId, timer);
            }
        }
        async static void aTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) //отправка новых вопросов на фронт
        {
            var timer = (CustomTimer)sender;
            IClientProxy hubClients = timer.hubCallerClients;
            string teamId = timer.team;
            Question question = timer.questions[timer.currentQuiestion];
            if(timer.questions.Count > timer.currentQuiestion + 1)
            {
                timer.currentQuiestion++;
            }
            else
            {
                timer.Elapsed -= aTimer_Elapsed;
                timer.Enabled = false;
            }
            await hubClients.SendAsync("NewQuestion", new
            {
                text = question.QuestionText,
                answers = timer.answers[timer.currentQuiestion].Select((answer) => new
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
            await _userAnswerRepository.Create(new UserAnswer { UserAnswerId = Guid.NewGuid(), Answer = answer, User = user, AnswerDate = DateTime.Now });
        }
        public async void TeamAnswer() //выбор ответа команды
        {

        }
        public async Task RoundOver(IClientProxy hubClients, string teamId)
        {
            Team team = await _teamRepository.GetTeamById(Guid.Parse(teamId));
            Round round = await _roundRepository.GetCurrentRoundByTeamName(team.TeamName);
            await _roundRepository.UpdataEndDate(round, DateTime.Now);
            await hubClients.SendAsync("RoundEnded");
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {

            return base.OnDisconnectedAsync(exception);
        }
    }
}
