using Prolance.Domain.Entities;

namespace Prolance.Infrastructure.Persistence.Repositories
{
    public interface IBidRepository
    {
        Task<List<Bid>> GetTodayBidsAsync(int accountId, string userId);
        Task AddBidAsync(Bid bid);
        Task<List<Account>> GetAccountsAsync();
    }
}
