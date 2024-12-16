using ExpenseVoid.Interface;
using ExpenseVoid.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

// Fetching Exchange Rates from Turkish Central Bank 

namespace ExpenseVoid.Provider
{
    public class TcbExchangeProvider : ICurrency
    {
        #region Fields

        private readonly ILogger<TcbExchangeProvider> _logger;

        #endregion

        #region Ctor

        public TcbExchangeProvider(ILogger<TcbExchangeProvider> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task<IList<Currency>> GetCurrencyLiveRatesAsync(string exchangeRateCurrencyCode)
        {
            if (string.IsNullOrEmpty(exchangeRateCurrencyCode))
                throw new ArgumentNullException(exchangeRateCurrencyCode, nameof(exchangeRateCurrencyCode));

            //add TRY with rate 1
            var ratesToTr = new List<Currency>
        {
            new Currency
            {
                CurrencyCode = "TRY",
                Value = 1,
                LastModifiedDate = DateTime.UtcNow
            }
        };

            //get exchange rates to TRY from Turkish Central Bank
            try
            {
                var httpClient = new HttpClient();
                var stream = await httpClient.GetStreamAsync("http://www.tcmb.gov.tr/kurlar/today.xml");

                //load XML document
                var document = new XmlDocument();
                document.Load(stream);


                //get daily rates
                var dailyRates = document.SelectSingleNode("Tarih_Date");
                if (!DateTime.TryParseExact(dailyRates.Attributes["Tarih"].Value, "dd.MM.yyyy", null, DateTimeStyles.None, out var updateDate))
                    updateDate = DateTime.UtcNow;

                foreach (XmlNode currency in dailyRates.ChildNodes)
                {
                    //get rate
                    if (!decimal.TryParse(currency["BanknoteSelling"].InnerText, NumberStyles.Currency, CultureInfo.InvariantCulture, out var currencyRate))
                        continue;

                    if (!int.TryParse(currency["Unit"].InnerText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var unit))
                        continue;

                    ratesToTr.Add(new Currency()
                    {
                        CurrencyCode = currency.Attributes["Kod"].Value,
                        Value = currencyRate / unit,
                        LastModifiedDate = updateDate
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TCB exchange rate provider");
            }

            //return result for the TRY
            if (exchangeRateCurrencyCode.Equals("try", StringComparison.InvariantCultureIgnoreCase))
                return ratesToTr;

            //use only currencies that are supported by TCB
            var exchangeRateCurrency = ratesToTr.FirstOrDefault(rate => rate.CurrencyCode.Equals(exchangeRateCurrencyCode, StringComparison.InvariantCultureIgnoreCase));
            if (exchangeRateCurrency == null)
                throw new Exception("You can use TCB (Turkish Central Bank) exchange rate provider only when the primary exchange rate currency is supported by TCB.");

            //return result for the selected (not TRY) currency
            return ratesToTr.Select(rate => new Currency
            {
                CurrencyCode = rate.CurrencyCode,
                Value = Math.Round(exchangeRateCurrency.Value / rate.Value, 4),
                LastModifiedDate = rate.LastModifiedDate
            }).ToList();
        }

        #endregion
    }
}
