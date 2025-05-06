using System.Threading.Tasks;

namespace LibraryManagement.Core.Interfaces.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendEmailVerificationAsync(string email, string token);
        Task SendPasswordResetAsync(string email, string token);
    }
}