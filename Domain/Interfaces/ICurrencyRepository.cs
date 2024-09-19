using Prolance.Domain.Entities;

namespace Prolance.Domain.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<List<Currency>> GetAllAsync();
        Task<Currency> GetByIdAsync(int id);
        Task AddCurrencyAsync(string currencyCode);
        Task UpdateAllExchangeRatesAsync();
    }
}
