namespace _002_Notes_App_03.DTOs
{
    public class PagedResponse<T>
    {
        public List<T> Items = new List<T>();
        public int Page;
        public int PageSize;
        public int TotalItems;
        public int TotalPages;
    }
}
