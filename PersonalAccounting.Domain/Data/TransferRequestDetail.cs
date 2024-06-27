using System.Diagnostics.Metrics;

namespace PersonalAccounting.Domain.Data
{
    public class TransferRequestDetail
    {
        public int Id { get; set; } 
        public int TransferRequestId { get; set; } 

        public TransferRequest TransferRequest { get; set; } 

        public string Comment { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public string CreateUserId { get; set; } = string.Empty;
        public string CreateUserName { get; set; } = string.Empty;
        public string CreateUserFullName { get; set; } = string.Empty;

    }
}
