using System.Collections.Generic;

namespace PersonalAccounting.BlazorApp.Components.Reports
{
    public class SharedExpensesReportModel
    {
        public Dictionary<string, decimal> SharedAmounts { get; set; } = new Dictionary<string, decimal>();
    }
}
