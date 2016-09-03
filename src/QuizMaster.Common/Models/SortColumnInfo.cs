namespace QuizMaster.Common.Models
{
    public class SortColumnInfo
    {
        public SortColumnInfo(string column, SortingOrder sortingOrder)
        {
            Column = column;
            SortingOrder = sortingOrder;
        }

        public string Column { get; set; }
        public SortingOrder SortingOrder { get; set; }
    }
}
