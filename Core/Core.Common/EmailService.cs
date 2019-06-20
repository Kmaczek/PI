using Core.Common;
using Core.Model.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace Common
{
    public class EmailService : IEmailService
    {
        private const string PersonalEmailKey = "PI_PersonalEmail";
        private const string SystemEmailKey = "PI_SystemEmail";
        private const string SystemEmailPassKey = "PI_SystemEmailPass";
        private const string GmailPortKey = "emailService:gmailPort";
        private const string GmailHostKey = "emailService:gmailHost";

        private readonly string personalEmail;
        private readonly string systemEmail;
        private readonly string systemEmailPass;
        private readonly int GmailPort;
        private readonly string GmailHost;
        private readonly IConfigurationRoot _configuration;
        private string defaultTitle = "Daily Summary for {0}";

        public EmailService(IConfigurationRoot configuration)
        {
            this._configuration = configuration;
            personalEmail = configuration.GetSection(PersonalEmailKey).Value;
            if (string.IsNullOrEmpty(personalEmail))
                throw new EmailServiceException($"Missing configuration for Email Service [{PersonalEmailKey}]");

            systemEmail = configuration.GetSection(SystemEmailKey).Value;
            if (string.IsNullOrEmpty(systemEmail))
                throw new EmailServiceException($"Missing configuration for Email Service [{SystemEmailKey}]");

            systemEmailPass = configuration.GetSection(SystemEmailPassKey).Value;
            if (string.IsNullOrEmpty(systemEmailPass))
                throw new EmailServiceException($"Missing configuration for Email Service [{SystemEmailPassKey}]");

            if (string.IsNullOrEmpty(configuration.GetSection(GmailPortKey).Value))
                throw new EmailServiceException($"Missing configuration for Email Service [{GmailPortKey}]");
            GmailPort = Convert.ToInt32(configuration.GetSection(GmailPortKey).Value, CultureInfo.InvariantCulture);

            GmailHost = configuration.GetSection(GmailHostKey).Value;
            if (string.IsNullOrEmpty(GmailHost))
                throw new EmailServiceException($"Missing configuration for Email Service [{GmailHostKey}]");
        }

        public void SendEmail(string body, string title = null)
        {
            string subject;
            if (title == null)
                subject = String.Format(CultureInfo.InvariantCulture, defaultTitle, DateTime.Now.ToString("dd-MM-yyy", CultureInfo.InvariantCulture));
            else
                subject = title;

            MailMessage mail = new MailMessage(systemEmail, personalEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            mail.From = new MailAddress(systemEmail, "DK Automation");
            SendMessage(mail);
        }

        private void SendMessage(MailMessage message)
        {
            using (var mailingClient = new SmtpClient())
            using (message)
            {
                mailingClient.Port = GmailPort;
                mailingClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailingClient.UseDefaultCredentials = false;
                mailingClient.Host = GmailHost;
                mailingClient.EnableSsl = true;
                mailingClient.Credentials = new NetworkCredential(systemEmail, systemEmailPass);

                mailingClient.Send(message);
            }
        }
    }
}
