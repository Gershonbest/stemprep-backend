using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Common.Enum;
using Infrastructure.Templates;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;
        private readonly IConfiguration _configuration;
        private readonly string _password;
        private readonly string _smtpServer;
        private readonly int _port;

        public EmailService(IConfiguration configuration)
        {
            IConfigurationSection emailConfig = configuration.GetSection("Mail");
            _fromEmail = emailConfig["Email"];
            _password = emailConfig["Password"];
            _smtpServer = emailConfig["SmtpServer"];


            if (!int.TryParse(emailConfig["Port"], out _port))
            {
                throw new InvalidOperationException("Invalid port value in configuration");
            }

            _smtpClient = new SmtpClient(_smtpServer, _port)
            {
                Credentials = new NetworkCredential(_fromEmail, _password),
                EnableSsl = true,
                UseDefaultCredentials = false
            };
        }

        public async Task<Result> SendEmailAsync(string toEmail, string subject, string body)
        {
            MailMessage mailMessage = new(_fromEmail, toEmail, subject, body);
            return await SendEmail(toEmail, mailMessage);
        }

        public async Task<Result> SendPasswordResetCodeAsync(string toEmail, string code)
        {
            string subject = "Password Reset Verification Code";
            string formattedBody = $@"
                                    <p>Hello!</p>
                                    <p>Enter this verification code to confirm your e-mail account to reset your password:</p>
                                    <p class=""code"">{code}</p>
                                    <p>Verification codes will expire soon.</p>
                                    ";
            string formattedHtml = ReplaceTemplateVariables(subject, formattedBody);

            MailMessage mailMessage = new(_fromEmail, toEmail, subject, formattedHtml)
            {
                IsBodyHtml = true
            };

            return await SendEmail(toEmail, mailMessage);
        }

        public async Task<Result> SendRegistrationConfirmationEmailAsync(string toEmail, string name, string code)
        {
            string subject = "Registration Confirmation Code";
            string formattedBody = $@"
                                    <p>Hello {name}!</p>
                                    <p>Your registration confirmation code is: </p>
                                    <p class=""code"">{code}</p>
                                    <p>Verification codes will expire soon.</p>
                                    ";
            string formattedHtml = ReplaceTemplateVariables(subject, formattedBody);

            MailMessage mailMessage = new(_fromEmail, toEmail, subject, formattedHtml)
            {
                IsBodyHtml = true
            };

            return await SendEmail(toEmail, mailMessage);
        }

        public async Task<Result> SendAccountConfirmationCodeAsync(string toEmail, string code)
        {
            string subject = "Account Confirmation Code";
            string formattedBody = $@"
                                    <p>Hello!</p>
                                    <p>Enter this verification code to confirm your account</p>
                                    <p class=""code"">{code}</p>
                                    <p>Verification codes will expire soon.</p>
                                    ";
            string formattedHtml = ReplaceTemplateVariables(subject, formattedBody);

            MailMessage mailMessage = new(_fromEmail, toEmail, subject, formattedHtml)
            {
                IsBodyHtml = true
            };

            return await SendEmail(toEmail, mailMessage);
        }

        public async Task<Result> SendLoginDetailsAsync(string toEmail, string accountType, string password)
        {
            string subject = accountType + " Login Details";
            string formattedBody = $@"
                                    <p>Hello!</p>
                                    <p>Your {accountType} account has been created.</p>
                                    <p>Here are your Login Details:</p>
                                    <p class=""code"">Email: {toEmail}<br>
                                                      Password: {password}</p>
                                    ";
            string formattedHtml = ReplaceTemplateVariables(subject, formattedBody);

            MailMessage mailMessage = new(_fromEmail, toEmail, subject, formattedHtml)
            {
                IsBodyHtml = true
            };

            return await SendEmail(toEmail, mailMessage);
        }

        private static string ReplaceTemplateVariables(string subject, string formattedBody)
        {
            string htmlTemplate = EmailTemplate.GetTemplate();
            string formattedHtml = $@"{htmlTemplate}";
            formattedHtml = formattedHtml.Replace("{subject}", subject);
            formattedHtml = formattedHtml.Replace("{body}", formattedBody);

            return formattedHtml;
        }
        private async Task<Result> SendEmail(string toEmail, MailMessage mailMessage)
        {
            _smtpClient.Host = _smtpServer;
            _smtpClient.Port = _port;
            _smtpClient.Credentials = new NetworkCredential(_fromEmail, _password);
            _smtpClient.EnableSsl = true;

            try
            {
                await _smtpClient.SendMailAsync(mailMessage);
                return Result.Success($"Email successfully sent to {toEmail}");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to send email to {toEmail}", ex);
            }
        }
    }
}