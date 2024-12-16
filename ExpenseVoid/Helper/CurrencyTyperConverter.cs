using ExpenseVoid.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseVoid.Helper
{
    public class CurrencyTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string stringValue)
            {
                // Split the string into parts
                var parts = stringValue.Split(' ');

                // Ensure the format matches "CurrencyCode Value"
                if (parts.Length == 2 && decimal.TryParse(parts[1], NumberStyles.Currency, culture ?? CultureInfo.InvariantCulture, out var currencyValue))
                {
                    return new Currency
                    {
                        CurrencyCode = parts[0].Trim(),
                        Value = currencyValue
                    };
                }

                throw new FormatException($"Invalid format for currency conversion: {stringValue}. Expected format is 'CurrencyCode Value' (e.g., 'USD 1.25').");
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is Currency currency)
            {
                return $"{currency.CurrencyCode} {currency.Value}";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

}
