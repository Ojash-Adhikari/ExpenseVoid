using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseVoid.Models
{
    public class Group
    {
        public Guid GroupID { get; set; } = Guid.NewGuid();
        public User? User { get; set; }
        public string? GroupName { get; set; } // Cash, BankAccount, Asset, Credit
    }
}
