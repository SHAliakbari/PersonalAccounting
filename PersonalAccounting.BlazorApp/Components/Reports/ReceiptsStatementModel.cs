using PersonalAccounting.BlazorApp.Components.Receipt_Component.Services;

namespace PersonalAccounting.Domain.Reports
{
    public class ReceiptsStatementModel
    {
        public string UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ReceiptReportItem> ReceiptReportItems { get; set; }
    }
}
