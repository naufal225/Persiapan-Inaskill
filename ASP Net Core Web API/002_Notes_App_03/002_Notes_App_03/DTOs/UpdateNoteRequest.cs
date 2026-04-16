using System.ComponentModel.DataAnnotations;

namespace _002_Notes_App_03.DTOs
{
    public class UpdateNoteRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

    }
}
