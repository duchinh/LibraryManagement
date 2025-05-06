using System;
using System.Net.Mail;
using System.Threading.Tasks;
using LibraryManagement.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace LibraryManagement.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly bool _enableSsl;
        private readonly string _username;
        private readonly string _password;

        public EmailSender(IConfiguration configuration)
        {
            _smtpServer = configuration["EmailSettings:SmtpServer"] ?? throw new ArgumentNullException("SmtpServer");
            _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"] ?? throw new ArgumentNullException("SmtpPort"));
            _enableSsl = bool.Parse(configuration["EmailSettings:EnableSsl"] ?? "false");
            _username = configuration["EmailSettings:Username"] ?? throw new ArgumentNullException("Username");
            _password = configuration["EmailSettings:Password"] ?? throw new ArgumentNullException("Password");
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                EnableSsl = _enableSsl,
                Credentials = new NetworkCredential(_username, _password)
            };

            var message = new MailMessage
            {
                From = new MailAddress(_username),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(to));

            await client.SendMailAsync(message);
        }

        public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, string attachmentPath)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_username),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(to));
            message.Attachments.Add(new Attachment(attachmentPath));

            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                EnableSsl = _enableSsl,
                Credentials = new NetworkCredential(_username, _password)
            };

            await client.SendMailAsync(message);
        }

        public async Task SendBorrowApprovedEmailAsync(string to, string bookTitle, string dueDate)
        {
            var subject = "Yêu cầu mượn sách đã được duyệt";
            var body = $@"
                <h2>Yêu cầu mượn sách của bạn đã được duyệt</h2>
                <p>Sách: {bookTitle}</p>
                <p>Hạn trả: {dueDate}</p>
                <p>Vui lòng đến thư viện để nhận sách.</p>
            ";

            await SendEmailAsync(to, subject, body);
        }

        public async Task SendBorrowRejectedEmailAsync(string to, string bookTitle, string reason)
        {
            var subject = "Yêu cầu mượn sách bị từ chối";
            var body = $@"
                <h2>Yêu cầu mượn sách của bạn đã bị từ chối</h2>
                <p>Sách: {bookTitle}</p>
                <p>Lý do: {reason}</p>
            ";

            await SendEmailAsync(to, subject, body);
        }

        public async Task SendOverdueNotificationEmailAsync(string to, string bookTitle, string dueDate)
        {
            var subject = "Thông báo sách quá hạn trả";
            var body = $@"
                <h2>Thông báo sách quá hạn trả</h2>
                <p>Sách: {bookTitle}</p>
                <p>Hạn trả: {dueDate}</p>
                <p>Vui lòng trả sách sớm nhất có thể.</p>
            ";

            await SendEmailAsync(to, subject, body);
        }

        public async Task SendReturnConfirmationEmailAsync(string to, string bookTitle)
        {
            var subject = "Xác nhận trả sách";
            var body = $@"
                <h2>Xác nhận trả sách thành công</h2>
                <p>Sách: {bookTitle}</p>
                <p>Cảm ơn bạn đã sử dụng dịch vụ thư viện.</p>
            ";

            await SendEmailAsync(to, subject, body);
        }

        public async Task SendEmailVerificationAsync(string email, string token)
        {
            var subject = "Xác thực email";
            var body = $"Vui lòng xác thực email của bạn bằng cách nhấp vào liên kết sau: {token}";
            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetAsync(string email, string token)
        {
            var subject = "Đặt lại mật khẩu";
            var body = $"Vui lòng đặt lại mật khẩu của bạn bằng cách nhấp vào liên kết sau: {token}";
            await SendEmailAsync(email, subject, body);
        }
    }
}