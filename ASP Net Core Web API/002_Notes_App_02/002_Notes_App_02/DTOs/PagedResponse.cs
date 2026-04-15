namespace _002_Notes_App_02.DTOs
{
    public class PagedResponse<T>
    {
        public List<T>? Items { get; set; } = new List<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
