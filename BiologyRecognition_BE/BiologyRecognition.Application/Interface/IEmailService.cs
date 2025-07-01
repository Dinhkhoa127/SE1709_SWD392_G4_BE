using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);

        Task SendAccountCreationEmailAsync(string userName,string toEmail, string plainPassword);
        Task SendAccountRegisterEmailAsync(string userName, string toEmail, string fullName,DateTime createDate);

        Task SendResetPasswordEmailAsync(string toEmail, string resetLink);
    }
}
