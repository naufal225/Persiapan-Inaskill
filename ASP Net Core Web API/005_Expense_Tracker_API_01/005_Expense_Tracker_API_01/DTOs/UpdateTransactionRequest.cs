using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _005_Expense_Tracker_API_01.DTOs
{
    public class UpdateTransactionRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;
    }
}
