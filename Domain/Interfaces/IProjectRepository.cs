using Prolance.Application.DTOs;
using Prolance.Domain.Entities;

namespace Prolance.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync(int? accountId, string userEmail);
        Task<ProjectDto> GetProjectByIdAsync(int id);
        Task CreateProjectAsync(ProjectDto projectDto);
        Task UpdateProjectAsync(ProjectDto projectDto);
        Task DeleteProjectAsync(int id);
        Task<IEnumerable<BidDto>> SearchBidsAsync(string query);
        Task<double> CalculateBudgetAsync(double grossBudget, int currencyId, bool isRecruiter);
        Task<IEnumerable<CurrencyDto>> GetCurrenciesAsync();
    }
}
