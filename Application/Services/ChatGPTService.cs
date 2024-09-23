using PuppeteerSharp;

namespace Prolance.Application.Services
{
    public class ChatGPTService
    {
        public async Task<string> SendMessageToChatGPT(string userMessage)
        {
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = false }))
            using (var page = await browser.NewPageAsync())
            {
                await page.GoToAsync("https://chat.openai.com");
                await page.WaitForSelectorAsync(".chat-input");
                await page.TypeAsync(".chat-input", userMessage);
                await page.ClickAsync(".chat-send-button");
                await page.WaitForSelectorAsync(".chat-response");
                var response = await page.EvaluateExpressionAsync<string>("document.querySelector('.chat-response').innerText");
                return response;
            }
        }
    }
}
