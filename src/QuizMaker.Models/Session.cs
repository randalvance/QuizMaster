using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Models
{
    public class Session
    {
        public Session()
        {
            CorrectAnswerCount = 0;
            DateTaken = DateTime.Now;
            SessionStatus = SessionStatus.NotStarted;
            QuizSessions = new List<QuizSession>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SessionId { get; set; }
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<QuizSession> QuizSessions { get; set; }
        public List<SessionAnswer> SessionAnswers { get; set; }
        public SessionStatus SessionStatus { get; set; }
        public int CorrectAnswerCount { get; set; }
        public int QuizItemCount { get; set; }
        public DateTime DateTaken { get; set; }
        public DateTime? DateCompleted { get; set; }
    }
}
