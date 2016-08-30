using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace QuizMaker.Data.Abstractions
{
    public interface IListOptions<T>
    {
        IEnumerable<Expression<Func<T, object>>> Includes { get; set; }
    }
}
