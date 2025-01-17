using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalAccounting.Domain.Data
{
    public class Receipt : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Required]
        public TimeSpan Time { get; set; }

        [Required]
        public string MerchantName { get; set; } = string.Empty;

        [Required]
        public string ShopName { get; set; } = string.Empty;

        [Required]
        public decimal TotalAmount { get; set; }

        public decimal AdditionDeduction { get; set; } = 0;

        public string PaidByUserId { get; set; } = string.Empty;

        public string PaidByUserName { get; set; } = string.Empty;

        public string PaidByUserFullName { get; set; } = string.Empty;

        public string ImageFileName { get; set; } = string.Empty;

        public string AdditionalInformation { get; set; } = string.Empty;

        public byte[]? Thumbnail { get; set; }

        [NotMapped]
        public Dictionary<string, decimal> UserShares { get; set; } = new Dictionary<string, decimal>();

        // Navigation property for related items
        public List<ReceiptItem> Items { get; set; } = new List<ReceiptItem>(); // Initialize to avoid null exceptions

        public Dictionary<string, decimal> CalculateSharedAmounts()
        {
            var sharedAmounts = new Dictionary<string, decimal>();

            foreach (var item in Items)
            {
                foreach (var share in item.Shares)
                {
                    decimal amountOwed = item.TotalPrice * (share.Share / 100);
                    if (sharedAmounts.ContainsKey(share.UserName))
                    {
                        sharedAmounts[share.UserName] += amountOwed;
                    }
                    else
                    {
                        sharedAmounts[share.UserName] = amountOwed;
                    }
                }
            }

            return sharedAmounts;
        }
    }
}
