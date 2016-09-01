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

        public int Page { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 10;
        public IEnumerable<Expression<Func<T, object>>> Includes { get; set; }
    }
}
