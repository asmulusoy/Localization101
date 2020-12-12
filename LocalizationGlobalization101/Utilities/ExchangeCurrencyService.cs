using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace LocalizationGlobalization101.Utilities
{
    public class ExchangeCurrencyService
    {
        public List<ExchangeCurrency> Currencies { get; set; }
        public ExchangeCurrencyService()
        {
            var birinciMi = false;
            try
            {
                var url = "https://kur.doviz.com/";
                var web = new HtmlWeb();
                var doc = web.Load(url);
                var elements = doc.GetElementbyId("currencies").Element("tbody").ChildNodes.Where(x => x.Name == "tr").ToList();
                Currencies = new List<ExchangeCurrency>
                {
                    new ExchangeCurrency()
                    {
                        CurrencyName = "Türk Lirası", ForexBuying = 1, ForexSelling = 1, CurrencyCode = "TRY"
                    }
                };
                foreach (var htmlNode in elements)
                {
                    var dovizLine = htmlNode.ChildNodes.Where(x => x.Name == "td").ToList();
                    if (dovizLine.Count < 2) continue;

                    string currencyName = dovizLine[0].InnerText.Substring(0, dovizLine[0].InnerText.IndexOf('-') - 1);
                    string currencyCode = dovizLine[0].InnerText.Substring(dovizLine[0].InnerText.IndexOf('-') + 1).Trim();
                    string forexBuying = dovizLine[3].InnerText;
                    string forexSelling = dovizLine[4].InnerText;
                    var doviz = new ExchangeCurrency
                    {
                        CurrencyName = currencyName,
                        CurrencyCode = currencyCode,
                        //Saat = dovizLine[2].InnerText,
                        ForexBuying = decimal.Parse(forexBuying,new CultureInfo("tr-TR")),
                        ForexSelling = decimal.Parse(forexSelling, new CultureInfo("tr-TR")),
                        //Fark = double.Parse(dovizLine[5].InnerText.Replace("% ", ""))
                    };
                    Currencies.Add(doviz);
                }

                birinciMi = Currencies.Count > 0;
            }
            catch (Exception e)
            {


            }
            if (birinciMi) return;
            XElement kurlar = XElement.Load("http://www.tcmb.gov.tr/kurlar/today.xml");
            List<XElement> dovizler = (from k in kurlar.Elements()
                                       where k.Element("CurrencyName") != null
                                       select k).ToList();
            try
            {
                Currencies = new List<ExchangeCurrency>
                {
                    new ExchangeCurrency()
                    {
                        CurrencyName = "Türk Lirası", ForexBuying = 1, ForexSelling = 1, CurrencyCode = "TRY"
                    }
                };
                foreach (var item in dovizler)
                {
                    Currencies.Add(new ExchangeCurrency()
                    {
                        CurrencyName = item.Element("Isim").Value,
                        ForexBuying = Convert.ToDecimal(item.Element("ForexBuying").Value.Replace(".", ",")),
                        ForexSelling = Convert.ToDecimal(item.Element("ForexSelling").Value.Replace(".", ",")),
                        CurrencyCode = item.Attribute("Kod").Value
                    });
                }
            }
            catch { }
        }
    }
    public class ExchangeCurrency
    {
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ForexBuying { get; set; }
        public decimal ForexSelling { get; set; }
    }
}
