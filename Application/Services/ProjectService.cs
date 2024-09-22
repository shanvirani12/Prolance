using Prolance.Application.DTOs;
using Prolance.Infrastructure.Persistence.Data;
using Prolance.Domain.Interfaces;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using Prolance.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Prolance.Application.Services
{
    public class ProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync(int? accountId, string userEmail)
        {
            return await _projectRepository.GetAllProjectsAsync(accountId, userEmail);
        }

        public async Task<ProjectDto> GetProjectByIdAsync(int id)
        {
            return await _projectRepository.GetProjectByIdAsync(id);
        }

        public async Task CreateProjectAsync(ProjectDto projectDto)
        {
            await _projectRepository.CreateProjectAsync(projectDto);
        }

        public async Task UpdateProjectAsync(ProjectDto projectDto)
        {
            await _projectRepository.UpdateProjectAsync(projectDto);
        }

        public async Task DeleteProjectAsync(int id)
        {
            await _projectRepository.DeleteProjectAsync(id);
        }

        public async Task<IEnumerable<BidDto>> SearchBidsAsync(string query)
        {
            return await _projectRepository.SearchBidsAsync(query);
        }

        public async Task<(double netBudget, double budgetInPKR)> CalculateBudgetAsync(double grossBudget, int currencyId, bool isRecruiter)
        {
            // You can add additional business logic here if necessary
            var budgetData = await _projectRepository.CalculateBudgetAsync(grossBudget, currencyId, isRecruiter);

            // Return the calculated budget
            return budgetData;
        }

        public async Task<IEnumerable<CurrencyDto>> GetCurrenciesAsync()
        {
            return await _projectRepository.GetCurrenciesAsync();
        }
    }
}
