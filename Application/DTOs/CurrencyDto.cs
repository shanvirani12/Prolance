using System;

namespace Prolance.Application.DTOs
{
    public class CurrencyDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
