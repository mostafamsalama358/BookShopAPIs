using Domains;
using Domains.DTOS;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Runtime;
using static Domains.DTOS.PasswordRecoveryDto;
namespace Bl
{
    public class EmailServices : IEmailServices
    {
        private readonly EmailSettings _settings;
        public EmailServices(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmail(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_settings.Email);
            email.To.Add(MailboxAddress.Parse(mailRequest.Email));
            email.Subject = mailRequest.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = mailRequest.EmailBody
            };

            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            // Connect to the SMTP server
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);

            // Authenticate the user
            smtp.Authenticate(_settings.Email, _settings.Password); // No 'Async' here

            // Send the email
            await smtp.SendAsync(email);

            // Disconnect from the SMTP server
            await smtp.DisconnectAsync(true);
        }
    }
}
