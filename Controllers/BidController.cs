using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Prolance.Application.DTOs;
using Prolance.Application.Services;
using Prolance.Domain.Entities;

namespace Prolance.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BidController : Controller
    {
        private readonly BidService _bidService;
        private readonly UserManager<User> _userManager;

        public BidController(BidService bidService, UserManager<User> userManager)
        {
            _bidService = bidService;
            _userManager = userManager;
        }

        public async Task<IActionResult> SelectAccount()
        {
            var accounts = await _bidService.GetAccountsAsync();
            ViewBag.Accounts = accounts;
            return View(accounts);
        }

        [HttpPost]
        public async Task<IActionResult> SelectAccount(int accountId)
        {
            if (accountId > 0)
            {
                return RedirectToAction("Index", new { accountId });
            }

            var accounts = await _bidService.GetAccountsAsync();
            ViewBag.Accounts = accounts;
            ModelState.AddModelError("", "Please select a valid account.");
            return View();
        }

        public async Task<IActionResult> Index(int accountId)
        {
            if (accountId <= 0)
            {
                return RedirectToAction("SelectAccount");
            }

            var userId = _userManager.GetUserId(User);
            var todayBids = await _bidService.GetTodayBidsAsync(accountId, userId);
            ViewBag.AccountId = accountId;
            return View(todayBids);
        }

        [HttpPost]
        public async Task<IActionResult> AddBid(int accountId, string link)
        {
            var userId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                await _bidService.AddBidAsync(accountId, link, userId);
                return RedirectToAction(nameof(Index), new { accountId });
            }

            var todayBids = await _bidService.GetTodayBidsAsync(accountId, userId);
            ViewBag.AccountId = accountId;
            return View("Index", todayBids);
        }
    }
}
