using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalAccounting.Domain.Data;

namespace PersonalAccounting.BlazorApp.Components.Account
{
    public class CustomIdentityService
    {
        private readonly UserManager<ApplicationUser> UserManager;

        public CustomIdentityService(UserManager<ApplicationUser> UserManager)
        {
            this.UserManager=UserManager;
        }

        public async Task<bool> ValidateInviteCodeAsync(string inviteCode)
        {
            return UserManager.Users.Count() == 0 || await UserManager.Users.AnyAsync(x => x.InviteCode == inviteCode && x.IsInviteUsed == false);
        }
    }
}
