namespace _005_Expense_Tracker_API_01.DTOs
{
    public class GetTransactionQuery
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public string? Type { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;
        public string? Search = string.Empty;
        public string Sort = "desc";
    }
}
