using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalAccounting.Domain.Data
{
    public abstract class BaseEntity
    {
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public string CreateUserId { get; set; } = string.Empty;

        public string CreateUserName { get; set; } = string.Empty;

        public string CreateUserFullName { get; set; } = string.Empty;

        public DateTime? LastEditDate { get; set; }

        public string? LastEditUserId { get; set; }

        public string? LastEditUserName { get; set; } = string.Empty;

        public string? LastEditUserFullName { get; set; } = string.Empty;
    }
}
