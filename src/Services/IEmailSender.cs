using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularASPNETCore2WebApiAuth.Services
{
    /// <summary>
    /// IEmailSender emailSender has been registered as injected service, 
    /// await _emailSender.SendEmailAsync(Email, ....)
    /// </summary>
    public interface IEmailSender
    {
        Task SendEmailAsync(string emailAddress, string subject, string message);
    }
}
