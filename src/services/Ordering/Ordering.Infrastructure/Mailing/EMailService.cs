using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Mailing
{
    internal class EmailService : IEmailService
    {

        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, 
            ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings?.Value ?? throw new ArgumentNullException(nameof(emailSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Send(EmailMessage email)
        {
            using var client = CreateClient();
            using var message = CreateMessage(email);

            message.Body = email.Body;
            message.Subject = email.Subject;

            try
            {
                await client.SendMailAsync(message);
            }
            catch (Exception e)
            {
                _logger.LogError("Error sending email, error {0}", e.Message);
                return false;
            }

            _logger.LogInformation("Email {0} sent", email.Subject);
            return true;
        }

        private MailMessage CreateMessage(EmailMessage email)
        {
            var from = new MailAddress(_emailSettings.From, "отдел продаж");
            var to = new MailAddress(email.To);

            return new MailMessage(from, to);
        }

        private SmtpClient CreateClient()
        {
            const int SmtpPort = 465;
            return new SmtpClient("smtp.mail.ru", SmtpPort)
            {
                Credentials = new NetworkCredential(_emailSettings.From, _emailSettings.Password), 
                EnableSsl = true
            };                     
        }
    }
}
