using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseVoid.Models
{
    public class Profile
    {
        public Guid ProfileID { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public decimal Balance { get; set; }
        public Group? Group { get; set; }
        public User? User { get; set; }

        
    }
}
