using System.Collections.Generic;
using System.Linq;

namespace QuizMaker.Models.SessionViewModels
{
    public class SessionListViewModel
    {
        public List<SessionViewModel> Sessions { get; set; }
        public bool UserSpecified { get; set; }
        public double PassingGrade { get; set; }
        public int RequiredQuizes { get; set; }
        public int QuizesCompleted { get; set; }
        public int QuizesPassed { get; set; }
        public int QuizesFailed { get; set; }
        public double GradeAverage
        {
            get
            {
                return Sessions.Average(x => x.GradePercentage);
            }
        }
        public string Remark
        {
            get
            {
                return GradeAverage >= PassingGrade ? "Passed" : "Failed";
            }
        }
    }
}
