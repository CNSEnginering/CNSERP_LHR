using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Net.Mail;
using ERP.Configuration.Host.Dto;

namespace ERP.Configuration
{
    public abstract class SettingsAppServiceBase : ERPAppServiceBase
    {
        private readonly IEmailSender _emailSender;

        protected SettingsAppServiceBase(
            IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        #region Send Test Email

        public async Task SendTestEmail(SendTestEmailInput input)
        {
            await _emailSender.SendAsync(
                input.EmailAddress,
                L("TestEmail_Subject"),
                L("TestEmail_Body")
            );
        }






        #endregion

        //Added by waleed 14/05/2020
        public async Task SendMail(MailMessage mail,
             string host,
             int port,
             string userName,
             string password,
             string domain,
             bool enableSsl,
             bool useDefaultCredentials)
        {
            using (var smtpClient = new SmtpClient(host, port))
            {
                try
                {
                    if (enableSsl)
                    {
                        smtpClient.EnableSsl = true;
                    }

                    if (useDefaultCredentials)
                    {
                        smtpClient.UseDefaultCredentials = true;
                    }
                    else
                    {
                        smtpClient.UseDefaultCredentials = false;

                        if (!userName.IsNullOrEmpty())
                        {
                            smtpClient.Credentials = !domain.IsNullOrEmpty()
                                ? new NetworkCredential(userName, password, domain)
                                : new NetworkCredential(userName, password);
                        }
                    }

                    await smtpClient.SendMailAsync(mail);
                }
                catch (Exception ex)
                {
                    smtpClient.Dispose();
                }
            }
        }
    }
}
