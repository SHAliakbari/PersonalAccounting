using Microsoft.AspNetCore.Identity;

namespace PersonalAccounting.Domain.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Title { get; set; } = string.Empty;
    }

}
