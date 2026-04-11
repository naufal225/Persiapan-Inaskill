using System.ComponentModel.DataAnnotations;

namespace _001_Todo_App_01.Dtos
{
    public class CreateTodoRequest
    {
        [Required]
        [MinLength(1)]
        public string Title { get; set; } = string.Empty;
    }

    public class UpdateTodoRequest
    {
        [Required]
        [MinLength(1)]
        public string Title { get; set; } = string.Empty;
        public bool IsDone { get; set; }
    }
}
