using QuizMaster.Common.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuizMaster.Common
{
    public interface ISortManager
    {
        IQueryable<T> ApplySorting<T>(string sortString, IQueryable<T> itemsToSort);
        List<SortColumnInfo> ParseSortString(string sortString);
        string BuildSortString(List<SortColumnInfo> columnInfos);
    }
}
