using Prolance.Domain.Entities;

namespace Prolance.Application.DTOs
{
    public class BidDto
    {
        public int BidId { get; set; }
        public string Link { get; set; }
        public string UserName { get; set; }
        public string AccountName { get; set; }
        public DateTime DateTime { get; set; }
        public string userId { get; set; }
        public int accountId { get; set; }
        public User user { get; set; }
        public Account account { get; set; }
    }
}
