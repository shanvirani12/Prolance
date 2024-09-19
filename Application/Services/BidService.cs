using Prolance.Application.DTOs;
using Prolance.Domain.Entities;
using Prolance.Infrastructure.Persistence.Repositories;

namespace Prolance.Application.Services
{
    public class BidService
    {
        private readonly IBidRepository _bidRepository;

        public BidService(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        public async Task<List<BidDto>> GetTodayBidsAsync(int accountId, string userId)
        {
            var bids = await _bidRepository.GetTodayBidsAsync(accountId, userId);
            return bids.Select(b => new BidDto
            {
                BidId = b.BidId,
                Link = b.Link,
                AccountName = b.Account.Name != null ? b.Account.Name : "N/A",  // Check if Account is null
                UserName = b.User != null ? $"{b.User.FirstName} {b.User.LastName}" : "Unknown",  // Check if User is null
                DateTime = b.DateTime
            }).ToList();
        }

        public async Task AddBidAsync(int accountId, string link, string userId)
        {
            var bid = new Bid
            {
                AccountID = accountId,
                Link = link,
                UserId = userId,
                DateTime = DateTime.Now
            };

            await _bidRepository.AddBidAsync(bid);
        }

        public async Task<List<AccountDTO>> GetAccountsAsync()
        {
            var accounts = await _bidRepository.GetAccountsAsync();
            return accounts.Select(a => new AccountDTO
            {
                Id = a.Id,
                Name = a.Name,
                Email = a.Email,
            }).ToList();
        }
    }
}
