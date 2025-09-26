using Application.Common.Models;
namespace Application.Interfaces;
public interface IEmailService
{
    Task<Result> SendEmailAsync(string toEmail, string subject, string body);
    Task<Result> SendPasswordResetCodeAsync(string toEmail, string code);
    Task<Result> SendRegistrationConfirmationEmailAsync(string toEmail, string name,string code);
    Task<Result> SendAccountConfirmationCodeAsync(string toEmail, string code);
    Task<Result> SendPasswordResetEmailAsync(string toEmail, string link);
    Task<Result> SendLoginDetailsAsync(string toEmail, string accountType, string password);

}
