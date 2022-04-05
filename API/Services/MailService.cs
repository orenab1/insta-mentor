using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using API.Interfaces;
using API.Settings;
using DAL.DTOs;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.Win32.SafeHandles;
using MimeKit;

namespace API.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        private static void SendCompletedCallback(
            object sender,
            AsyncCompletedEventArgs e
        )
        {
            string token = (string) e.UserState;
        }

        public void SendEmailAsync(EmailDto emailDto)
        {
            string to = emailDto.To;
            string from = _mailSettings.Mail;
            string subject = emailDto.Subject;
            string body = emailDto.Body;
            MailMessage message = new MailMessage(from, to, subject, body);
            message.IsBodyHtml=true;
            SmtpClient client = new SmtpClient(_mailSettings.Host, _mailSettings.Port);
            client.EnableSsl = true;

            client.Credentials =
                new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
            client.Send (message);
        }
    }
}
