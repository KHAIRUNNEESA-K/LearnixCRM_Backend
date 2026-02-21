using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

public class SendGridEmailService : IEmailService
{
    private readonly SendGridSettings _settings;

    public SendGridEmailService(IOptions<SendGridSettings> options)
    {
        _settings = options.Value;

        Console.WriteLine("SendGrid API Key at runtime:");
        Console.WriteLine(_settings.ApiKey);
    }

    public async Task SendUserRegisteredAsync(string toEmail)
    {
        var client = new SendGridClient(_settings.ApiKey);

        var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
        var to = new EmailAddress(toEmail);

        var subject = "Registration received – Pending approval";

        var plainText =
            "Your registration was received and is pending admin approval.";

        var htmlContent = @"
        <h3>Registration Received</h3>
        <p>Thank you for registering with <b>LearnixCRM</b>.</p>
        <p>Your account is currently <b>pending admin approval</b>.</p>
        <p>You will receive another email once your account is approved.</p>";

        var msg = MailHelper.CreateSingleEmail(
            from, to, subject, plainText, htmlContent);

        var response = await client.SendEmailAsync(msg);

        if (response.StatusCode != HttpStatusCode.Accepted)
        {
            var responseBody = await response.Body.ReadAsStringAsync();

            throw new Exception(
                $"SendGrid FAILED | StatusCode: {response.StatusCode} | Response: {responseBody}"
            );
        }
    }


    public async Task SendApprovalEmailAsync(string toEmail, string setPasswordLink)
    {
        var client = new SendGridClient(_settings.ApiKey);

        var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
        var to = new EmailAddress(toEmail);

        var subject = "Your LearnixCRM account is approved";

        var htmlContent = $@"
        <h3>Account Approved</h3>
        <p>Your account has been approved.</p>
        <p>Click below to set your password:</p>
        <a href='{setPasswordLink}'>Set Password</a>
        <p><b>Note:</b> Link expires in 30 minutes.</p>";

        var msg = MailHelper.CreateSingleEmail(
            from, to, subject, htmlContent, htmlContent);

        var response = await client.SendEmailAsync(msg);
        var body = await response.Body.ReadAsStringAsync();

        if (response.StatusCode != HttpStatusCode.Accepted)
        {
            throw new Exception(
                $"SendGrid failed | Status: {response.StatusCode} | Body: {body}"
            );
        }

    }

    public async Task SendRejectionEmailAsync(string toEmail, string rejectReason)
    {
        var client = new SendGridClient(_settings.ApiKey);

        var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
        var to = new EmailAddress(toEmail);

        var subject = "Your LearnixCRM account registration has been rejected";

        var htmlContent = $@"
        <h3>Registration Rejected</h3>
        <p>We regret to inform you that your registration request has been rejected by the admin.</p>

        <p><strong>Reason for rejection:</strong></p>
        <p style='color:red;'>{rejectReason}</p>

        <p>If you believe this was a mistake, please contact support.</p>

        <br/>
        <p>Regards,<br/>LearnixCRM Team</p>";

        var msg = MailHelper.CreateSingleEmail(
            from,
            to,
            subject,
            "Your registration has been rejected.", 
            htmlContent);

        var response = await client.SendEmailAsync(msg);

        if (response.StatusCode != HttpStatusCode.Accepted)
            throw new Exception("Failed to send rejection email");
    }

    public async Task SendResetPasswordEmailAsync(string toEmail, string resetPasswordLink)
    {
        var client = new SendGridClient(_settings.ApiKey);

        var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
        var to = new EmailAddress(toEmail);

        var subject = "Reset your LearnixCRM password";

        var plainText = $@"
You requested a password reset.

Use the link below to reset your password:
{resetPasswordLink}

This link will expire in 30 minutes.
If you did not request this, please ignore this email.
";

        var htmlContent = $@"
        <h3>Password Reset Request</h3>
        <p>We received a request to reset your <b>LearnixCRM</b> password.</p>
        <p>Click the button below to reset your password:</p>
        <p>
            <a href='{resetPasswordLink}'
               style='padding:10px 16px;
                      background-color:#2563eb;
                      color:#ffffff;
                      text-decoration:none;
                      border-radius:6px;'>
                Reset Password
            </a>
        </p>
        <p><b>Note:</b> This link will expire in 30 minutes.</p>
        <p>If you didn’t request this, you can safely ignore this email.</p>
    ";

        var msg = MailHelper.CreateSingleEmail(
            from,
            to,
            subject,
            plainText,
            htmlContent
        );

        var response = await client.SendEmailAsync(msg);

        if (response.StatusCode != HttpStatusCode.Accepted)
            throw new Exception("Failed to send reset password email");
    }


}
