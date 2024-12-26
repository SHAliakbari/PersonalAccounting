using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public string? LastEditUserId { get; set; }

        public string? LastEditUserName { get; set; } = string.Empty;

        public string? LastEditUserFullName { get; set; } = string.Empty;

        public DateOnly RequestDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        public string FromUserId { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "The From field is not a valid e-mail address")]
        public string FromUserName { get; set; } = string.Empty;

        public string FromUserFullName { get; set; } = string.Empty;

        public string ToUserId { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "The To field is not a valid e-mail address")]
        public string ToUserName { get; set; } = string.Empty;

        public string ToUserFullName { get; set; } = string.Empty;

        public string ReceiverUserId { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "The Receiver field is not a valid e-mail address")]
        public string ReceiverUserName { get; set; } = string.Empty;

        public string ReceiverUserFullName { get; set; } = string.Empty;

        public string ReceiverAccountNo { get; set; } = string.Empty;

        public string ReceiverCardNo { get; set; } = string.Empty;

        public string ReceiverNote { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public string SourceCurrencyName { get; set; } = "CAD";

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [Range(1, 999999999, ErrorMessage = "Greater that zero")]
        public decimal SourceAmount { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public string DestinationCurrencyName { get; set; } = "IRR";

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [Range(1, 999999999, ErrorMessage = "Greater that zero")]
        public decimal DestinationAmount { get; set; }

        [Required(ErrorMessage = "Required")]
        public decimal ExchangeRate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public string FeeCurrencyName { get; set; } = "CAD";

        [Required(ErrorMessage = "Required")]
        public decimal Fee { get; set; }

        public string Status { get; set; } = "Pending";

        public ICollection<TransferRequestDetail> Details { get; set; }

        [NotMapped]
        public decimal Debit { get; set; }
        [NotMapped]
        public decimal Credit { get; set; }
        [NotMapped]
        public decimal RunningTotal { get; set; }


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
            if (ExchangeRate == 0) return;
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
