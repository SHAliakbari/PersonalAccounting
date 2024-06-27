namespace PersonalAccounting.Domain.Data
{
    public class TransferRequest
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public string CreateUserId { get; set; } = string.Empty;
        public string CreateUserName { get; set; } = string.Empty;
        public string CreateUserFullName { get; set; } = string.Empty;

        public DateTime? LastEditDate { get; set; }

        public string FromUserId { get; set; } = string.Empty;
        public string FromUserName { get; set; } = string.Empty;
        public string FromUserFullName { get; set; } = string.Empty;

        public string ToUserId { get; set; } = string.Empty;
        public string ToUserName { get; set; } = string.Empty;
        public string ToUserFullName { get; set; } = string.Empty;

        public string ReceiverUserId { get; set; } = string.Empty;
        public string ReceiverUserName { get; set; } = string.Empty;
        public string ReceiverUserFullName { get; set; } = string.Empty;

        public string ReceiverAccountNo { get; set; } = string.Empty;
        public string ReceiverCardNo { get; set; } = string.Empty;
        public string ReceiverNote { get; set; } = string.Empty;

        public string SourceCurrencyName { get; set; } = "CAD";
        public decimal SourceAmount { get; set; }

        public string DestinationCurrencyName { get; set; } = "IRR";
        public decimal DestinationAmount { get; set; }

        public decimal ExchangeRate { get; set; }
        public string FeeCurrencyName { get; set; } = "CAD";
        public decimal Fee { get; set; }

        public string Status { get; set; } = "Pending";

        public ICollection<TransferRequestDetail> Details { get; set; }


        public void CalculateDestinationAmount()
        {
            if (FeeCurrencyName  == SourceCurrencyName)
            {
                DestinationAmount = (SourceAmount - Fee) * ExchangeRate;
            }
            else
            {
                DestinationAmount = (SourceAmount * ExchangeRate) - Fee;
            }
        }

        public void CalculateSourceAmount()
        {
            if (FeeCurrencyName  == SourceCurrencyName)
            {
                SourceAmount = DestinationAmount / ExchangeRate + Fee;
            }
            else
            {
                SourceAmount = (DestinationAmount  + Fee)/ ExchangeRate;
            }
        }
    }
}
