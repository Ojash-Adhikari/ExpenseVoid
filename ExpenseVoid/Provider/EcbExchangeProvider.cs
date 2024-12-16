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

//Fetching Exchange Rates from European Central Bank (ECB)

namespace ExpenseVoid.Provider
{
    public class EcbExchangeProvider : ICurrency
    {
        #region Fields

        private readonly ILogger<EcbExchangeProvider> _logger;

        #endregion

        #region Ctor

        public EcbExchangeProvider(ILogger<EcbExchangeProvider> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task<IList<Currency>> GetCurrencyLiveRatesAsync(string exchangeRateCurrencyCode)
        {
            if (string.IsNullOrEmpty(exchangeRateCurrencyCode))
                throw new ArgumentNullException(exchangeRateCurrencyCode, nameof(exchangeRateCurrencyCode));

            //add euro with rate 1
            var ratesToEuro = new List<Currency>
        {
            new Currency
            {
                CurrencyCode = "EUR",
                Value = 1,
                LastModifiedDate = DateTime.UtcNow
            }
        };

            //get exchange rates to euro from European Central Bank
            try
            {
                var httpClient = new HttpClient();
                var stream = await httpClient.GetStreamAsync("http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml");

                //load XML document
                var document = new XmlDocument();
                document.Load(stream);

                //add namespaces
                var namespaces = new XmlNamespaceManager(document.NameTable);
                namespaces.AddNamespace("ns", "http://www.ecb.int/vocabulary/2002-08-01/eurofxref");
                namespaces.AddNamespace("gesmes", "http://www.gesmes.org/xml/2002-08-01");

                //get daily rates
                var dailyRates = document.SelectSingleNode("gesmes:Envelope/ns:Cube/ns:Cube", namespaces);
                if (!DateTime.TryParseExact(dailyRates.Attributes["time"].Value, "yyyy-MM-dd", null, DateTimeStyles.None, out var updateDate))
                    updateDate = DateTime.UtcNow;

                foreach (XmlNode currency in dailyRates.ChildNodes)
                {
                    //get rate
                    if (!decimal.TryParse(currency.Attributes["rate"].Value, NumberStyles.Currency, CultureInfo.InvariantCulture, out var currencyRate))
                        continue;

                    ratesToEuro.Add(new Currency()
                    {
                        CurrencyCode = currency.Attributes["currency"].Value,
                        Value = currencyRate,
                        LastModifiedDate = updateDate
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ECB exchange rate provider");
            }

            //return result for the euro
            if (exchangeRateCurrencyCode.Equals("eur", StringComparison.InvariantCultureIgnoreCase))
                return ratesToEuro;

            //use only currencies that are supported by ECB
            var exchangeRateCurrency = ratesToEuro.FirstOrDefault(rate => rate.CurrencyCode.Equals(exchangeRateCurrencyCode, StringComparison.InvariantCultureIgnoreCase));
            if (exchangeRateCurrency == null)
                throw new Exception("You can use ECB (European Central Bank) exchange rate provider only when the primary exchange rate currency is supported by ECB.");

            //return result for the selected (not euro) currency
            return ratesToEuro.Select(rate => new Currency
            {
                CurrencyCode = rate.CurrencyCode,
                Value = Math.Round(rate.Value / exchangeRateCurrency.Value, 4),
                LastModifiedDate = rate.LastModifiedDate
            }).ToList();
        }

        #endregion
    }
}
