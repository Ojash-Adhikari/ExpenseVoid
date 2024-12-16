using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseVoid.Models
{
    public class Tag
    {
        public Guid TagId { get; set; } = Guid.NewGuid();
        public string? TagName { get; set; }
        public string? TagType { get; set; } // refence to transaction type
        public string? TagColor { get; set; }
    }
}
