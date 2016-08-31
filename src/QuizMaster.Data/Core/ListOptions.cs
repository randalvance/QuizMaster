using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace QuizMaster.Data.Core
{
    public class ListOptions<T> 
    {
        public ListOptions(params Expression<Func<T, object>>[] includes)
        {
            Includes = includes;
        }

        public IEnumerable<Expression<Func<T, object>>> Includes { get; set; }
    }
}
