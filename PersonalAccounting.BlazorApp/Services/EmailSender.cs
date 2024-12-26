using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PersonalAccounting.Domain.Data;

namespace PersonalAccounting.BlazorApp.Services
{
    public class EmailSender : IEmailSender<ApplicationUser>
    {
        private readonly EmailService emailService;
        private readonly ILogger _logger;

        public EmailSender(EmailService emailService,
                           ILogger<EmailSender> logger)
        {
            this.emailService=emailService;
            _logger = logger;
        }

        public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            await SendEmailAsync(user.UserName ?? "", email, "Confirmation", confirmationLink);
        }

        public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            await SendEmailAsync(user.UserName ?? "", email, "Confirmation", $"resetCode = {resetCode}");
        }

        public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            await SendEmailAsync(user.UserName ?? "", email, "Confirmation", $"resetLink = {resetLink}");
        }

        private async Task SendEmailAsync(string toEmailName, string toEmail, string subject, string message)
        {
            await emailService.SendEmailAsync(toEmailName, toEmail, subject, message, null, null);

        }
    }
}
