using AutoMapper;
using Prolance.Application.DTOs;
using Prolance.Domain.Entities;
using Prolance.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prolance.Application.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountDTO>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AccountDTO>>(accounts);
        }

        public async Task<AccountDTO> GetAccountByIdAsync(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            return _mapper.Map<AccountDTO>(account);
        }

        public async Task AddAccountAsync(AccountDTO accountDto)
        {
            var account = _mapper.Map<Account>(accountDto);
            await _accountRepository.AddAsync(account);
        }

        public async Task UpdateAccountAsync(AccountDTO accountDto)
        {
            var account = await _accountRepository.GetByIdAsync(accountDto.Id);
            if (account != null)
            {
                _mapper.Map(accountDto, account);
                await _accountRepository.UpdateAsync(account);
            }
        }

        public async Task DeleteAccountAsync(int id)
        {
            await _accountRepository.DeleteAsync(id);
        }
    }
}
