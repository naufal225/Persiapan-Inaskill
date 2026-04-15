namespace _002_Notes_App_02.DTOs
{
    public class GetNotesQuery
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public string? Search { get; set; } = string.Empty;
        public string Sort { get; set; } = "desc";
    }
}
