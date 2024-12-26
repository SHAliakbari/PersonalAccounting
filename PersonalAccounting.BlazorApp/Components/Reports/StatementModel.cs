using PersonalAccounting.Domain.Data;

namespace PersonalAccounting.Domain.Reports
{
    public class StatementModel
    {
        public List<TransferRequest> Requests { get; set; }
    }
}
