using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net;

namespace AngularASPNETCore2WebApiAuth.Services
{
    public class MessageSender : IEmailSender
    {

        private string smtpAddress;
        private string senderAddress;
        private string senderName;
        private int port;

        public MessageSender()
        {
            smtpAddress = "smtp.gmail.com";
            senderAddress = "cgjames2008@gmail.com";
            senderName = "James";
            port = 587;
        }

        public async Task SendEmailAsync(string emailAddress, string subject, string message)
        {
            var emailMessage = new MailMessage();

            emailMessage.From = new MailAddress("cgjames2008@gmail.com");
            emailMessage.To.Add(emailAddress);
            emailMessage.IsBodyHtml = true;
            emailMessage.Body = message;
            emailMessage.Subject = subject;

            var client = new SmtpClient(smtpAddress)
            {
              Port = port,
              EnableSsl = true,
              UseDefaultCredentials = false,
              DeliveryMethod = SmtpDeliveryMethod.Network,
              Credentials = new NetworkCredential("cgjames2008@gmail.com", "jk8VFjm3")
            };

            await client.SendMailAsync(emailMessage);
        }
    }
}
