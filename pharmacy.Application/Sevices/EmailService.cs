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
            message.Subject = "Reset Your Password";
            message.Body = new TextPart("html")
            {
                Text = $@"
            <h2>Reset Your Password</h2>
            <p>Click the link below to reset your password:</p>
            <a href='{resetLink}' style='background:#2563eb; color:white; padding:12px 24px; border-radius:8px; text-decoration:none;'>
                Reset Password
            </a>
            <p>This link expires in 1 hour.</p>"
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync("sandbox.smtp.mailtrap.io", 2525, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("0b5589bb5dba26", "326365d5919dfe");
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}