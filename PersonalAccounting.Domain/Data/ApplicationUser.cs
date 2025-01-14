using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalAccounting.Domain.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? InviteCode { get; set; }
        public bool IsInviteUsed { get; set; } = false;

        public string TelegramUser { get; set; } = string.Empty;

        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string FullName { get; set; } = string.Empty;

        public string? AccountNo { get; set; }
        public string? CardNo { get; set; }
    }
}
