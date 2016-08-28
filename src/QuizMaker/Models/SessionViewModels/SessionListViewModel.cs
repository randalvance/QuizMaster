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
                var grades = Sessions.Select(x => x.GradePercentage).ToList();

                if (grades.Count < RequiredQuizes)
                {
                    var sessionsToAdd = RequiredQuizes - grades.Count;
                    var dummyScores = new double[sessionsToAdd];
                    grades.AddRange(dummyScores);
                }

                return grades.Average();
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
