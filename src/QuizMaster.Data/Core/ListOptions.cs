using QuizMaker.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace QuizMaker.Data.Core
{
    public class ListOptions<T> : IListOptions<T>
    {
        public ListOptions(params Expression<Func<T, object>>[] includes)
        {
            Includes = includes;
        }

        public IEnumerable<Expression<Func<T, object>>> Includes { get; set; }
    }
}
