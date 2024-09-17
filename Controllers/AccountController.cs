using Microsoft.AspNetCore.Mvc;
using Prolance.Application.DTOs;
using Prolance.Application.Services;
using System.Threading.Tasks;

namespace Prolance.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return View(accounts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountDTO accountDto)
        {
            if (ModelState.IsValid)
            {
                await _accountService.AddAccountAsync(accountDto);
                return RedirectToAction(nameof(Index));
            }
            return View(accountDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null) return NotFound();

            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AccountDTO accountDto)
        {
            if (ModelState.IsValid)
            {
                await _accountService.UpdateAccountAsync(accountDto);
                return RedirectToAction(nameof(Index));
            }
            return View(accountDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null) return NotFound();

            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _accountService.DeleteAccountAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
