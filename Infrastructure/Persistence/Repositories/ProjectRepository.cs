using AutoMapper;
using Prolance.Application.DTOs;
using Prolance.Domain.Interfaces;
using Prolance.Infrastructure.Persistence.Data;
using Prolance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Prolance.Infrastructure.Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProjectRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync(int? accountId, string userEmail)
        {
            var query = _context.Projects
                .Include(p => p.Account)
                .Include(p => p.User)
                .Include(p => p.Currency)
                .AsQueryable();

            if (accountId.HasValue)
            {
                query = query.Where(p => p.AccountID == accountId.Value);
            }

            if (!string.IsNullOrEmpty(userEmail))
            {
                query = query.Where(p => p.User.Email == userEmail);
            }

            var projects = await query.ToListAsync();
            return _mapper.Map<List<ProjectDto>>(projects);
        }

        public async Task<ProjectDto> GetProjectByIdAsync(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Account)
                .Include(p => p.User)
                .Include(p => p.Currency)
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task CreateProjectAsync(ProjectDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(ProjectDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CurrencyDto>> GetCurrenciesAsync()
        {
            var currencies = await _context.Currencies.ToListAsync();
            return _mapper.Map<List<CurrencyDto>>(currencies);
        }


        public async Task<IEnumerable<BidDto>> SearchBidsAsync(string query)
        {
            return await _context.Bids
                .Include(b => b.User)
                .Include(b => b.Account)
                .Where(b => b.Link.Contains(query))
                .Select(b => new BidDto
                {
                    BidId = b.BidId,
                    Link = b.Link,
                    UserName = b.User.UserName,
                    AccountName = b.Account.Name
                })
                .ToListAsync();
        }

        public async Task<double> CalculateBudgetAsync(double grossBudget, int currencyId, bool isRecruiter)
        {
            var currency = await _context.Currencies.FindAsync(currencyId);
            if (currency == null)
            {
                throw new Exception("Currency not found.");
            }

            double platformFee = isRecruiter ? grossBudget * 0.15 : grossBudget * 0.10;
            double netBudget = grossBudget - platformFee;
            double budgetInPKR = netBudget * (double)currency.ExchangeRate;

            return budgetInPKR;
        }
    }
}
