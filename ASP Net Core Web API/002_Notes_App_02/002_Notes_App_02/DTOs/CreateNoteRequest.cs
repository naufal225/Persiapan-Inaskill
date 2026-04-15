using System.ComponentModel.DataAnnotations;

namespace _002_Notes_App_02.DTOs
{
    public class CreateNoteRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
