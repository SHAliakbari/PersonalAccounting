using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Category { get; set; } = string.Empty;

        [Required]
        public decimal TotalAmount { get; set; }

        public string PaidByUserId { get; set; } = string.Empty;

        public string PaidByUserName { get; set; } = string.Empty;

        public string PaidByUserFullName { get; set; } = string.Empty;

        public string ImageFileName { get; set; } = string.Empty;

        public string AdditionalInformation { get; set; } = string.Empty;

        public byte[]? Thumbnail { get; set; }

        // Navigation property for related items
        public List<ReceiptItem> Items { get; set; } = new List<ReceiptItem>(); // Initialize to avoid null exceptions
    }
}
