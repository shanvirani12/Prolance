using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Prolance.Application.DTOs;
using Prolance.Application.Services;

namespace Prolance.Controllers
{
    [Authorize(Roles = "Admin")]
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
            ViewBag.Currencies = new SelectList(await _projectService.GetCurrenciesAsync(), "Id", "Code");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectDto projectDto)
        {
            await _projectService.CreateProjectAsync(projectDto);
            return RedirectToAction(nameof(Index));            
        }

        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();
            ViewBag.Currencies = new SelectList(await _projectService.GetCurrenciesAsync(), "Id", "Code", project.CurrencyId);
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
            ViewBag.Currencies = new SelectList(await _projectService.GetCurrenciesAsync(), "Id", "Code", projectDto.CurrencyId);
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
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SearchBids(string query)
        {
            var bids = await _projectService.SearchBidsAsync(query);
            return Json(bids);
        }

        [HttpGet]
        public async Task<IActionResult> CalculateBudget(double grossBudget, int currencyId, bool isRecruiter)
        {
            try
            {
                // Call the service to get the calculated budget
                var result = await _projectService.CalculateBudgetAsync(grossBudget, currencyId, isRecruiter);

                // Return the result as JSON
                return Ok(new
                {
                    success = true,
                    netBudget = result.netBudget,
                    budgetInPKR = result.budgetInPKR
                });
            }
            catch (Exception ex)
            {
                // Handle any errors and return a JSON error response
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

    }

}
