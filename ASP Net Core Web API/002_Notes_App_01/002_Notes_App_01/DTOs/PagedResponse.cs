namespace _002_Notes_App_01.DTOs
{
    public class PagedResponse<T>
    {
        public List<T> Items = new List<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPage { get; set; }
    }
}
