namespace _004_Notes_API_Database_Async_01.DTOs
{
    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new();
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}
