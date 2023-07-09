using LearningApp.Application.Interfaces;
using LearningApp.Application.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace LearningApp.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(SmtpSettings smtpSettings)
        {
            _smtpSettings = smtpSettings;
        }

        public async Task SendAccountVerificationEmail(string userEmail)
        {
            var email = new MimeMessage();
            var link = "https://www.google.pl";

            email.From.Add(MailboxAddress.Parse(_smtpSettings.EmailUsername));
            email.To.Add(MailboxAddress.Parse(userEmail));
            email.Subject = "Welcome to LearningApp - please verify your email";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = "<h1>Welcome&nbsp;to LearningApp</h1>\r\n\r\n" +
                       "<p>Please click the link below to verify your email address." +
                       "</p>\r\n\r\n<p>" +
                       $"<a href=\"{link}\" target=\"_blank\">" +
                       "Verify email</a>" +
                       "</p>\r\n\r\n<p>" +
                       "The link is valid for 24 hours.</p>\r\n\r\n<hr />\r\n<p>All the best,<br />\r\nThe LearningApp team</p>"
            };
            email.Cc.Add(MailboxAddress.Parse(_smtpSettings.EmailUsername));

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpSettings.EmailHost, 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtpSettings.EmailUsername, _smtpSettings.EmailPassword);
            await smtp.SendAsync(email);
        }
    }
}
