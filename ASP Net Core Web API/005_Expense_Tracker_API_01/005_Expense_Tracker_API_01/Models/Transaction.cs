using System.ComponentModel.DataAnnotations.Schema;

namespace _005_Expense_Tracker_API_01.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]

        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty; // income / expense
        public string Category { get; set; } = string.Empty; // salary / food/ transport
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
