using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseVoid.Models
{
    public class Transaction
    {
       
        public Guid TransactionID { get; set; } = Guid.NewGuid();
        public User? User { get; set; }
        public Tag? Tag { get; set; }
        public double? TransactionAmount { get; set; }
        public string? TransactionName { get; set; }
        public string? TransactionRemarks { get; set; }
        public TransactionType? TransactionType { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? TransactionColor { get; set; }

        

    }
}
