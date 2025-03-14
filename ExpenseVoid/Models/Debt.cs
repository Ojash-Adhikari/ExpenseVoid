using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseVoid.Models
{
    public class Debt
    {
        public Guid DebtId { get; set; } = Guid.NewGuid();
        public User? User { get; set; }
        public Source? Source { get; set; }
        public double? Amount { get; set; }
        public DateTime? BorrowedDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? DueDate { get;  set; }
        public Boolean? IsCleared { get; set; }
        public string? Notes { get; set; }
        public string? DebtColor { get; set; }

       
    }
}
