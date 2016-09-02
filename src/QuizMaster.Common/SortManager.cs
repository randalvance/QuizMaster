using QuizMaster.Common.Extensions;
using QuizMaster.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QuizMaster.Common
{
    public class SortManager : ISortManager
    {
        public IQueryable<T> ApplySorting<T>(string sortString, IQueryable<T> itemsToSort)
        {
            if (string.IsNullOrWhiteSpace(sortString))
            {
                return itemsToSort;
            }

            
            IOrderedQueryable<T> orderedQueryable = null;
            var isFirst = true;

            var columnInfos = ParseSortString(sortString);

            foreach (var columnInfo in columnInfos)
            {
                Expression<Func<T, object>> sortingExpression = x => x.GetPropertyValue(columnInfo.Column);

                if (columnInfo.SortingOrder == SortingOrder.Ascending)
                {
                    orderedQueryable = isFirst ? itemsToSort.OrderBy(sortingExpression) : 
                        orderedQueryable.ThenBy(sortingExpression);
                }
                else if(columnInfo.SortingOrder == SortingOrder.Descending)
                {
                    orderedQueryable = isFirst ? itemsToSort.OrderByDescending(x => x.GetPropertyValue(columnInfo.Column)) :
                        orderedQueryable.ThenByDescending(sortingExpression);
                }

                isFirst = false;
            }

            return orderedQueryable == null ? itemsToSort : orderedQueryable;
        }

        public List<SortColumnInfo> ParseSortString(string sortString)
        {
            if (string.IsNullOrWhiteSpace(sortString))
            {
                return new List<SortColumnInfo>();
            }

            var sortColumnInfos = new List<SortColumnInfo>();
            var sortColumnsSplits = sortString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var sortColumn in sortColumnsSplits)
            {
                var tokens = sortColumn.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
                var column = tokens[0];
                var ascDesc = tokens[1];
                var sortingOrder = SortingOrder.Ascending; 

                if (ascDesc.ToLower() == "asc")
                {
                    sortingOrder = SortingOrder.Ascending;
                }
                else if(ascDesc.ToLower() == "desc")
                {
                    sortingOrder = SortingOrder.Descending;
                }
                else
                {
                    throw new ArgumentException($"Invalid sort order key {ascDesc} found for {column}");
                }

                sortColumnInfos.Add(new SortColumnInfo(column, sortingOrder));
            }

            return sortColumnInfos;
        }
    }
}
