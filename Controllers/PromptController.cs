using Microsoft.AspNetCore.Mvc;
using Prolance.Application.Services;
using Prolance.Domain.Interfaces;

namespace Prolance.Controllers
{
    public class PromptController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ChatGPTService _chatGPTService;

        public PromptController(IAccountRepository accountRepository, ChatGPTService chatGPTService)
        {
            _accountRepository = accountRepository;
            _chatGPTService = chatGPTService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return View(accounts);
        }

        [HttpPost]
        public async Task<IActionResult> GetResponse(int accountId, string userInput)
        {
            var prompt = "Your task involves understanding the project details and outlining the project's basis. Additionally, you need to specify the steps you'll take to achieve the project's goals. It's important to emphasize your specific experience in the niche relevant to this project and highlight past successes. Make a commitment to completing the project within the agreed-upon timeline, and offer to work for free if this isn't achieved. The text should be unique and to the point, capturing client's attention. Also put one line on how we are gonna achieve it.\r\n\r\nTone should be: Spartan, Less corporate jargon, Conversational, Human touch, Easy English, Friendly.\r\n\r\nExample Bid: I have expertise in revitalizing e-commerce brands, creating vibrant online identities that drive sales. With a history of elevating visual branding, I assure timely delivery of captivating designs. Focusing on e-commerce entrepreneurs, I ensure heightened brand visibility and boosted conversions within 90 days. If targets aren't reached, I commit to working without charge until your objectives are fulfilled.\r\n\r\nI want Specific Target Audience, Promise. Timeline and Risk Reversal. But don't mention there heading or keywords, 1 Paragraph with 80 Words.";
            var account = await _accountRepository.GetByIdAsync(accountId);
            var combinedInput = $"{prompt}\nUser: {userInput}";

            var chatGPTResponse = await _chatGPTService.SendMessageToChatGPT(combinedInput);

            ViewBag.Response = chatGPTResponse;
            ViewBag.UserInput = userInput;
            ViewBag.SelectedPrompt = prompt;

            var accounts = await _accountRepository.GetAllAsync();
            ViewBag.Accounts = accounts; // Pass all accounts back to the view

            return View("Index"); // Return to Index view
        }
    }
}
