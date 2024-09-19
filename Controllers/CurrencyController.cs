using Microsoft.AspNetCore.Mvc;
using Prolance.Application.DTOs;
using Prolance.Application.Services;
using System.Threading.Tasks;

namespace Prolance.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly CurrencyService _currencyService;

        public CurrencyController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<IActionResult> Index()
        {
            var currencies = await _currencyService.GetAllCurrenciesAsync();
            return View(currencies);
        }

        [HttpPost]
        public async Task<IActionResult> AddCurrency(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode) || currencyCode.Length != 3)
            {
                TempData["Error"] = "Invalid currency code.";
                return RedirectToAction(nameof(Index));
            }

            await _currencyService.AddCurrencyAsync(currencyCode);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRates()
        {
            await _currencyService.UpdateAllExchangeRatesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
