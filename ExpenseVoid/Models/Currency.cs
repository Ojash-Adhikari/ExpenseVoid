using ExpenseVoid.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseVoid.Models
{
    [TypeConverter(typeof(CurrencyTypeConverter))]
    public class Currency
    {
        public Guid CurrencyID { get; set; } = Guid.NewGuid();
        public string CurrencyCode { get; set; }

        public decimal Value { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public Currency()
        {
            Value = 1.0m;
        }

        public override string ToString()
        {
            return $"{CurrencyCode} ({Value})";
        }
    }
}
