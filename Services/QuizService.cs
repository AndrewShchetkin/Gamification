using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamification.Utilities.Timers;
using Gamification.Models;
using Microsoft.AspNetCore.SignalR;
using Gamification.Hubs;
using Gamification.Data.Interfaces;
using Gamification.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Services
{
    public delegate void QuizDelegate(QuizEventArgs eventArgs);
    public delegate void RoundOverDelegate(RoundOverEventArgs eventArgs);
    
    public class QuizService
    {
        public event QuizDelegate SendNewQuestionNeeded;
        public event RoundOverDelegate RoundOver;
        private readonly ApplicationContext db;
        public Dictionary<string, TeamTimer> teamTimers { get; set; }
        public Dictionary<string, List<Question>> teamQuestions { get; set; }
        public Dictionary<string, List<List<Answer>>> teamAnswers { get; set; }
        public Dictionary<string, IClientProxy> teamUsers { get; set; }
        public Dictionary<string, int> currentTeamQuestion { get; set; }

        public QuizService(ApplicationContext applicationContext)
        {
            RoundOver += QuizHub.RoundOver;
            SendNewQuestionNeeded += QuizHub.NewQuestion;
            teamTimers = new Dictionary<string, TeamTimer>();
            teamQuestions = new Dictionary<string, List<Question>>();
            teamAnswers = new Dictionary<string, List<List<Answer>>>();
            teamUsers = new Dictionary<string, IClientProxy>();
            currentTeamQuestion = new Dictionary<string, int>();

            db = applicationContext;

            
        }
        

        private void NewQuestionHandler(object sender, System.Timers.ElapsedEventArgs e)
        {
            TeamTimer teamTimer = (TeamTimer)sender;
            string teamId = teamTimer.teamId;
            int currentQuestion = currentTeamQuestion[teamId];
            
            if(teamQuestions[teamId].Count > currentQuestion)
            {
                SendNewQuestionNeeded(new QuizEventArgs { answers = teamAnswers[teamId][currentTeamQuestion[teamId]],
                    question = teamQuestions[teamId][currentTeamQuestion[teamId]],
                    users = teamUsers[teamId],
                    qurrentQuestion = currentQuestion });
                currentTeamQuestion[teamId]++;
            }
            else
            {
                teamTimer.Dispose();
            }
        }

        private void RoundOverHandler(object sender, EventArgs eventArgs)
        {
            TeamTimer teamTimer = (TeamTimer)sender;
            RoundOver(new RoundOverEventArgs { teamId = teamTimer.teamId, applicationContext = db});
            ClearRoundInformation(teamTimer.teamId);
        }

        public void ClearRoundInformation(string teamId)
        {
            teamTimers.Remove(teamId);
            teamQuestions.Remove(teamId);
            teamAnswers.Remove(teamId);
            teamTimers.Remove(teamId);
            teamTimers.Remove(teamId);
        }

        public void InitializeRound(string teamId, List<Question> questions, List<List<Answer>> answers, IClientProxy users, int delay)
        {
            TeamTimer timer = new TeamTimer(delay);
            timer.Elapsed += NewQuestionHandler;
            timer.Disposed += RoundOverHandler;
            timer.teamId = teamId;
            teamTimers[teamId] = timer;
            teamQuestions[teamId] = questions;
            teamAnswers[teamId] = answers;
            currentTeamQuestion[teamId] = 0;
            teamUsers[teamId] = users;
            teamTimers[teamId].Start();
        }

    }
    public class QuizEventArgs : EventArgs
    {
        public Question question;
        public List<Answer> answers;
        public int qurrentQuestion;
        public IClientProxy users;
    }
    public class RoundOverEventArgs : EventArgs
    {
        public string teamId;
        public ApplicationContext applicationContext;
    }
}
