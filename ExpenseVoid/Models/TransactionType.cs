using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseVoid.Models
{
    public class TransactionType
    {
        public Guid TypeID { get; set; } = Guid.NewGuid();
        public Double? Credit { get; set; }
        public Double? Debit { get; set; }
        public Double? Debt { get; set; }


        //a action field can be created for extra protection
    }
}
