using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalAccounting.Domain.Data
{
    public class ReceiptItemShare
    {
        public int Id { get; set; }
        
        public string UserId { get; set; } 
        public string UserName { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;

        public decimal Share { get; set; } // Percentage or amount

        public int ReceiptItemId { get; set; } // Foreign Key
        public ReceiptItem ReceiptItem { get; set; } // Navigation property
    }
}
