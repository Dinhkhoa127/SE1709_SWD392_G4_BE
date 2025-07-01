using BiologyRecognition.Application.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Runtime;
using BiologyRecognition.DTOs.Email;

namespace BiologyRecognition.Application.Implement
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using var smtpClient = new SmtpClient(_settings.SmtpServer)
            {
                Port = _settings.SmtpPort,
                Credentials = new NetworkCredential(_settings.SenderEmail, _settings.SenderPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
        public async Task SendAccountCreationEmailAsync(string userName, string toEmail, string plainPassword)
        {
            var subject = "[Biology Recognition System] Tài khoản của bạn đã được tạo";
            var body = $@"
            <p>Chào Mừng bạn đến với hệ thống BiologyRecognition.</p>
            <p>Vui lòng không chia sẽ thông tin dưới đây cho bất kì ai.</p>
            <p>Tài khoản của bạn đã được tạo thành công:</p>
            <ul>
                <li><b>User Name:</b> {userName} </li>
                <li><b>Email:</b> {toEmail} </li>
                <li><b>Mật khẩu:</b> {plainPassword} </li>
            </ul>
            <p>Vui lòng đăng nhập và đổi mật khẩu ngay sau lần đăng nhập đầu tiên.</p>";

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendAccountRegisterEmailAsync(string userName, string toEmail, string fullName, DateTime createDate)
        {
            var subject = "[Biology Recognition System] Tài khoản của bạn đã được tạo";
            var body = $@"
            <p>Chào Mừng bạn đến với hệ thống BiologyRecognition.</p>
            <p>Vui lòng không chia sẽ thông tin dưới đây cho bất kì ai.</p>
            <p>Tài khoản của bạn đã được tạo thành công:</p>
            <ul>
                <li><b>User Name:</b> {userName} </li>
                <li><b>Email:</b> {toEmail} </li>
                <li><b>FullName:</b> {fullName} </li>
                <li><b>Date:</b> {createDate} </li>  
            </ul>
            <p>Vui lòng đăng nhập và kiểm tra thông tin ngay lần đăng nhập đầu tiên.</p>";

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendResetPasswordEmailAsync(string toEmail, string resetLink)
        {
            var subject = "[Biology Recognition System] Yêu cầu đặt lại mật khẩu";
            var body = $@"
            <p>Bạn đã yêu cầu đặt lại mật khẩu.</p>
            <p>Vui lòng click vào link dưới đây để tiếp tục:</p>
            <p><a href='{resetLink}'>Đặt lại mật khẩu</a></p>
            <p>Nếu bạn không yêu cầu việc này, hãy bỏ qua email này.</p>";

            await SendEmailAsync(toEmail, subject, body);
        }
    }
}
