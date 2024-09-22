using Prolance.Domain.Entities;
using System;

namespace Prolance.Application.DTOs
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectClientName { get; set; }
        public string ProjectLink { get; set; }
        public string ProjectType { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int AccountID { get; set; }
        public Account Account { get; set; }
        public DateTime AwardDate { get; set; }
        public bool IsRecruiter { get; set; }
        public string Status { get; set; }
        public double GrossBudget { get; set; }
        public double NetBudget { get; set; }
        public double BudgetInPKR { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public DateTime? ClosingDate { get; set; }
        public string AssignedTo { get; set; }
        public double? CostinPKR { get; set; }
    }
}
