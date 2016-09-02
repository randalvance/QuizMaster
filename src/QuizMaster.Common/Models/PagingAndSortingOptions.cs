namespace QuizMaster.Common.Models
{
    public class PagingAndSortingOptions
    {
        public int Page { get; set; } = 1;

        public int ItemsPerPage { get; set; } = 10;

        /// <summary>
        /// Pattern: Column1-ASC,Column2-DESC
        /// </summary>
        public string SortExpression { get; set; }
    }
}
