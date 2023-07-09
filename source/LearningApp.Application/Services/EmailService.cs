using LearningApp.Application.Interfaces;
using LearningApp.Application.Settings;
using LearningApp.Domain.Entities;
using LearningApp.Domain.Exceptions;
using LearningApp.Infrastructure.Persistence;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;

namespace LearningApp.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly LearningAppDbContext _dbContext;

        public EmailService(SmtpSettings smtpSettings, LearningAppDbContext dbContext)
        {
            _smtpSettings = smtpSettings;
            _dbContext = dbContext;
        }

        public async Task SendAccountVerificationEmail(string userEmail)
        {
            var verificationToken = await _dbContext.Users
                .Where(x => x.EmailAddress == userEmail)
                .Select(x => x.VerificationToken)
                .FirstOrDefaultAsync();

            if (verificationToken is null) throw new NotFoundException(nameof(User));

            var email = new MimeMessage();
            var link = $"{_smtpSettings.ApplicationUrl}api/Account/verify?verificationToken={verificationToken}";

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
