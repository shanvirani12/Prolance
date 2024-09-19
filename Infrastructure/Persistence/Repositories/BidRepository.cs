using Prolance.Domain.Entities;
using Prolance.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Prolance.Infrastructure.Persistence.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly ApplicationDbContext _context;

        public BidRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Bid>> GetTodayBidsAsync(int accountId, string userId)
        {
            return await _context.Bids
                .Include(b => b.Account)  // Eager load Account
                .Include(b => b.User)
                .Where(b => b.AccountID == accountId && b.UserId == userId && b.DateTime.Date == DateTime.Today)
                .OrderByDescending(b => b.DateTime)
                .ToListAsync();
        }

        public async Task AddBidAsync(Bid bid)
        {
            await _context.Bids.AddAsync(bid);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Account>> GetAccountsAsync()
        {
            return await _context.Accounts.ToListAsync();
        }
    }
}
