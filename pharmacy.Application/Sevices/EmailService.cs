using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace pharmacy.Application.Sevices
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string toEmail, string resetLink);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
        {
            var message = new MimeMessage();

            message.From.Add(MailboxAddress.Parse(_config["Email:From"]));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "Reset Password";

            message.Body = new TextPart("html")
            {
                Text = $"Click here to reset password: <a href='{resetLink}'>Reset</a>"
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _config["Email:SmtpHost"],
                int.Parse(_config["Email:SmtpPort"]),
                SecureSocketOptions.StartTls
            );

           
            var user = _config["Email:SmtpUser"]?.Trim();
            var pass = _config["Email:SmtpPass"]?.Trim();

            await smtp.AuthenticateAsync(user, pass);

            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}