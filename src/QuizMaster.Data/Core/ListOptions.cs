using QuizMaster.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace QuizMaster.Data.Core
{
    public class ListOptions<T> 
    {
        public ListOptions(params Expression<Func<T, object>>[] includes)
            : this(null, includes)
        {

        }

        public ListOptions(PagingAndSortingOptions pagingAndSorting, params Expression<Func<T, object>>[] includes)
        {
            Includes = includes;
            PagingAndSorting = pagingAndSorting;
        }

        public PagingAndSortingOptions PagingAndSorting { get; set; } = new PagingAndSortingOptions();

        public IEnumerable<Expression<Func<T, object>>> Includes { get; set; }
    }
}
