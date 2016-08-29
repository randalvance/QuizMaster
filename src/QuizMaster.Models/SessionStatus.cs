using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public enum SessionStatus : byte
    {
        NotStarted = 0,
        Ongoing = 1,
        Done = 2,
        Skipped = 3
    }
}
