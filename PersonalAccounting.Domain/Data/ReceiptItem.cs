using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalAccounting.Domain.Data
{
    public class ReceiptItem
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal TotalPrice { get; set; } 

        // Foreign key to the Receipt
        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; } // Navigation property
    }
}
