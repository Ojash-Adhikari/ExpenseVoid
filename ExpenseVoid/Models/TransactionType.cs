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
        public string? Name { get; set; }
        //a action field can be created for extra protection
    }
}
