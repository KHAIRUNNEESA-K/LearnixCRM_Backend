using LearnixCRM.Application.Interfaces;
using LearnixCRM.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace LearnixCRM.Infrastructure.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly SendGridSettings _settings;

        public SendGridEmailService(IOptions<SendGridSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendInviteAsync(string toEmail, string inviteLink)
        {
            try
            {
                var client = new SendGridClient(_settings.ApiKey);

                var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
                var to = new EmailAddress(toEmail);
                var subject = "You are invited to LearnixCRM";

                var plainTextContent = $"You are invited to LearnixCRM. Use this link to accept the invitation: {inviteLink}";
                var htmlContent = $@"
                    <h3>Welcome to LearnixCRM</h3>
                    <p>You have been invited to join LearnixCRM.</p>
                    <p>Click below to set your password:</p>
                    <a href='{inviteLink}'>Accept Invitation</a>
                    <p><b>Note:</b> This link expires in 48 hours.</p>";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

                var response = await client.SendEmailAsync(msg);
                var responseBody = await response.Body.ReadAsStringAsync();
                Console.WriteLine($"SendGrid Status: {response.StatusCode}");
                Console.WriteLine($"SendGrid Response: {responseBody}");

                if (response.StatusCode != HttpStatusCode.Accepted)
                {
                    throw new Exception(
                        $"Failed to send invitation email. StatusCode: {response.StatusCode}. Response: {responseBody}");
                }

                Console.WriteLine($"Invitation successfully sent to {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email to {toEmail}: {ex.Message}");
                throw; 
            }
        }

        public async Task SendResetPasswordAsync(string toEmail, string resetLink)
        {
            try
            {
                var client = new SendGridClient(_settings.ApiKey);

                var from = new EmailAddress(_settings.FromEmail,_settings.FromName);
                var to = new EmailAddress(toEmail);
                var subject = "Reset Your LearnixCRM Password";

                var plainTextContent = $"You requested a password reset. Use this link to reset your password: {resetLink}";
                var htmlContent = $@"
                      <h3>Password Reset Request</h3>
                      <p>You requested to reset your password.</p>
                      <p>Click below to reset your password:</p>
                      <a href='{resetLink}'>Reset Password</a>
                      <p><b>Note:</b> This link expires in 30 minutes.</p>";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

                var response = await client.SendEmailAsync(msg);

                var responseBody = await response.Body.ReadAsStringAsync();
                Console.WriteLine($"SendGrid Status: {response.StatusCode}");
                Console.WriteLine($"SendGrid Response: {responseBody}");

                if (response.StatusCode != HttpStatusCode.Accepted)
                {
                    throw new Exception(
                        $"Failed to send password reset email. StatusCode: {response.StatusCode}. Response: {responseBody}");
                }

                Console.WriteLine($"Password reset email successfully sent to {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending password reset email to {toEmail}: {ex.Message}");
                throw;
            }
        }
    }
}
