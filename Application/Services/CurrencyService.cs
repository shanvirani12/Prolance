using AutoMapper;
using Prolance.Application.DTOs;
using Prolance.Domain.Interfaces;

namespace Prolance.Application.Services
{
    public class CurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        public CurrencyService(ICurrencyRepository currencyRepository, IMapper mapper)
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        public async Task<List<CurrencyDto>> GetAllCurrenciesAsync()
        {
            var currencies = await _currencyRepository.GetAllAsync();
            return currencies.Select(c => _mapper.Map<CurrencyDto>(c)).ToList();
        }

        public async Task<CurrencyDto> GetCurrencyByIdAsync(int id)
        {
            var currency = await _currencyRepository.GetByIdAsync(id);
            return _mapper.Map<CurrencyDto>(currency);
        }

        public async Task AddCurrencyAsync(string currencyCode)
        {
            // You can delegate the API fetching and processing logic to a method in the repository
            await _currencyRepository.AddCurrencyAsync(currencyCode);
        }

        public async Task UpdateAllExchangeRatesAsync()
        {
            await _currencyRepository.UpdateAllExchangeRatesAsync();
        }
    }
}
