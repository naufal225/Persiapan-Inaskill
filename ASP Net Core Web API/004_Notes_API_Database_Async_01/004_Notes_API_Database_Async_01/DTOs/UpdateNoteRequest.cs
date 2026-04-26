using System.ComponentModel.DataAnnotations;

namespace _004_Notes_API_Database_Async_01.DTOs
{
    public class UpdateNoteRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
