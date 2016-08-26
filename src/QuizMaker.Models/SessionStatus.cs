using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Models
{
    public enum SessionStatus : byte
    {
        NotStarted = 0,
        Active = 1,
        Done = 2
    }
}
