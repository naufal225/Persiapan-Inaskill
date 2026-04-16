namespace _002_Notes_App_03.DTOs
{
    public class GetNotesQuery
    {
        public int Page = 1;
        public int PageSize = 2;
        public string? Search = string.Empty;
        public string Sort = "desc";
    }
}
