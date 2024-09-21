using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Prolance.Application.DTOs;
using Prolance.Application.Services;

namespace Prolance.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ProjectService _projectService;

        public ProjectsController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task<IActionResult> Index(int? accountId, string userEmail)
        {
            var projects = await _projectService.GetAllProjectsAsync(accountId, userEmail);
            return View(projects);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["CurrencyId"] = new SelectList(await _projectService.GetCurrenciesAsync(), "CurrencyId", "CurrencyCode");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectDto projectDto)
        {
            if (ModelState.IsValid)
            {
                await _projectService.CreateProjectAsync(projectDto);
                return RedirectToAction(nameof(Index));
            }
            return View(projectDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();
            ViewData["CurrencyId"] = new SelectList(await _projectService.GetCurrenciesAsync(), "CurrencyId", "CurrencyCode", project.CurrencyId);
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectDto projectDto)
        {
            if (id != projectDto.ProjectId) return NotFound();
            if (ModelState.IsValid)
            {
                await _projectService.UpdateProjectAsync(projectDto);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CurrencyId"] = new SelectList(await _projectService.GetCurrenciesAsync(), "CurrencyId", "CurrencyCode", projectDto.CurrencyId);
            return View(projectDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();
            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _projectService.DeleteProjectAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SearchBids(string query)
        {
            var bids = await _projectService.SearchBidsAsync(query);
            return Json(bids);
        }

        public async Task<IActionResult> CalculateBudget(double grossBudget, int currencyId, bool isRecruiter)
        {
            var budgetInPKR = await _projectService.CalculateBudgetAsync(grossBudget, currencyId, isRecruiter);
            return Json(new { budgetInPKR });
        }
    }

}
