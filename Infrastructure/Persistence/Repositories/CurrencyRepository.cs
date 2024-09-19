using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Prolance.Domain.Entities;
using Prolance.Domain.Interfaces;
using Prolance.Infrastructure.Persistence.Data;

namespace Prolance.Infrastructure.Persistence.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ApplicationDbContext _context;
        private const string ApiUrl = "https://api.exchangerate-api.com/v4/latest/USD"; // Base API URL

        public CurrencyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Currency>> GetAllAsync()
        {
            return await _context.Currencies.ToListAsync();
        }

        public async Task<Currency> GetByIdAsync(int id)
        {
            return await _context.Currencies.FindAsync(id);
        }

        public async Task AddCurrencyAsync(string currencyCode)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(ApiUrl);
                    var data = JsonConvert.DeserializeObject<dynamic>(response);
                    var rates = data.rates;

                    if (rates[currencyCode] == null)
                    {
                        throw new KeyNotFoundException("Currency code not found in the API.");
                    }

                    var exchangeRateToPKR = Convert.ToDecimal(rates.PKR) / Convert.ToDecimal(rates[currencyCode]);

                    var currency = await _context.Currencies.FirstOrDefaultAsync(c => c.Code == currencyCode);
                    if (currency == null)
                    {
                        currency = new Currency
                        {
                            Code = currencyCode,
                            ExchangeRate = exchangeRateToPKR,
                            LastUpdated = DateTime.Now
                        };
                        _context.Currencies.Add(currency);
                    }
                    else
                    {
                        throw new InvalidOperationException("Currency already exists.");
                    }

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Handle or log exception as needed
                    throw new ApplicationException($"Error adding currency: {ex.Message}", ex);
                }
            }
        }

        public async Task UpdateAllExchangeRatesAsync()
        {
            var currencies = await _context.Currencies.ToListAsync();
            foreach (var currency in currencies)
            {
                await UpdateExchangeRateAsync(currency.Code);
            }
        }

        private async Task UpdateExchangeRateAsync(string currencyCode)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(ApiUrl);
                    var data = JsonConvert.DeserializeObject<dynamic>(response);
                    var rates = data.rates;

                    var exchangeRateToPKR = Convert.ToDecimal(rates.PKR) / Convert.ToDecimal(rates[currencyCode]);

                    var currency = await _context.Currencies.FirstOrDefaultAsync(c => c.Code == currencyCode);
                    if (currency != null)
                    {
                        currency.ExchangeRate = exchangeRateToPKR;
                        currency.LastUpdated = DateTime.Now;
                        _context.Currencies.Update(currency);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Handle or log exception as needed
                    throw new ApplicationException($"Error updating exchange rate for {currencyCode}: {ex.Message}", ex);
                }
            }
        }
    }
}
