using LocalizationGlobalization101.Enums;
using LocalizationGlobalization101.Models;
using LocalizationGlobalization101.Utilities;
using LocalizationGlobalization101.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace LocalizationGlobalization101.Controllers
{
    public class HomeController : Controller
    {
        private readonly ExchangeCurrencyService _currencyService;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IStringLocalizer<HomeController> localizer, ExchangeCurrencyService currencyService)
        {
            _localizer = localizer;
            _currencyService = currencyService;
        }


        public IActionResult Index(DateTime? Date, int? DateTimeOffset)
        {
            var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var currentCulture = rqf.RequestCulture.Culture;
            //Birim Fiyat TL'dir
            var amount = 500;

            var dollar = _currencyService.Currencies.First(x => x.CurrencyCode == "Amerikan Doları").ForexSelling;
            var euro = _currencyService.Currencies.First(x => x.CurrencyCode == "Euro").ForexSelling;

            if (currentCulture.Name.StartsWith("en"))
                ViewBag.Currency = (amount / dollar).ToString("c", new CultureInfo("en-US"));
            else if (currentCulture.Name.StartsWith("fr"))
                ViewBag.Currency = (amount / euro).ToString("c", new CultureInfo("fr-FR"));
            else if (currentCulture.Name.StartsWith("tr"))
                ViewBag.Currency = amount.ToString("c", new CultureInfo("tr-TR"));


            if (Date.HasValue && DateTimeOffset.HasValue)
            {
                ViewBag.UTCDate = DatetimeUtil.FromUTCData(Date.Value, DateTimeOffset.Value);
            }


            ViewBag.Date = DateTime.Now;
            ViewBag.ControllerMessage = _localizer["ControllerTest"];

            ViewData["genders"] = new SelectList(MUsefulMethods.EnumHelpers.ToKeyValuePair<Gender>(), "Key", "Value");


            return View(new ProductViewModel());
        }

        
        public IActionResult DataAnnotationLocalization()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DataAnnotationLocalization(ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return View();

        }

        [HttpPost]
        public IActionResult Index(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CultureManagement(string c, string rUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(c)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });
            if (rUrl.Split("/").Length > 2)
            {
                rUrl = rUrl.Replace(rUrl.Split("/")[1], c);
            }
            else
            {
                rUrl = $"~/{c}";
            }
            return LocalRedirect(rUrl);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
