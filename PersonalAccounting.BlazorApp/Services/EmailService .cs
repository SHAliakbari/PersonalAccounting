using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace PersonalAccounting.BlazorApp.Services
{
    public class EmailService
    {
        private readonly EmailConfig emailConfig;
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            this.emailConfig = new EmailConfig
            {
                Username = Environment.GetEnvironmentVariable($"EmailConfig_UserName{GlobalConfigs.ProdSuffix}")!,
                Password= Environment.GetEnvironmentVariable($"EmailConfig_Password{GlobalConfigs.ProdSuffix}")!,
                Port =int.Parse(Environment.GetEnvironmentVariable($"EmailConfig_SMTPPort{GlobalConfigs.ProdSuffix}")!),
                SmtpServer = Environment.GetEnvironmentVariable($"EmailConfig_SMTPServer{GlobalConfigs.ProdSuffix}")!
            };
            _logger = logger;
        }

        public virtual async Task<bool> SendEmailAsync(string emailName, string email, string subject, string body, Dictionary<string, byte[]> InlineImagesBytes, Dictionary<string, string> InlineImages, Dictionary<string, byte[]>? attachments = null)
        {
            bool result = true;
            _logger.LogDebug($"Send Email to {email}");
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailConfig.Username, emailConfig.Username));
            message.To.Add(new MailboxAddress(emailName, email));
            message.Subject = subject;

            var builder = new BodyBuilder();

            builder.HtmlBody = body;
            if (InlineImages != null)
            {
                foreach (var inline in InlineImages)
                {
                    var attachment = builder.LinkedResources.Add(inline.Value);
                    attachment.ContentId = inline.Key;
                }
            }

            if (InlineImagesBytes != null)
            {
                foreach (var inline in InlineImagesBytes)
                {
                    var attachment = builder.LinkedResources.Add(inline.Key, inline.Value);
                    attachment.ContentId = inline.Key;
                }
            }

            if (attachments != null)
            {
                foreach (var file in attachments)
                    builder.Attachments.Add(file.Key, file.Value);
            }

            message.Body = builder.ToMessageBody();

            try
            {


                using (var client = new SmtpClient())
                {
                    //client.SslProtocols = System.Security.Authentication.SslProtocols.;
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    //client.Authenticate(
                    client.Connect(emailConfig.SmtpServer, emailConfig.Port, MailKit.Security.SecureSocketOptions.Auto);

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(emailConfig.Username, emailConfig.Password);

                    var res = await client.SendAsync(message);
                    _logger.LogInformation($"Send Result : {res}");
                    client.Disconnect(true);
                }
            }
            catch
            {
                throw;
            }
            return result;
        }
    }
}
