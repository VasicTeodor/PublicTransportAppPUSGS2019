using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace PublicTransport.Api.Helpers
{
    public static class EmailService
    {
        public async static void SendEmail(string text, string toEmail)
        {
            var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("tranda96srk@gmail.com", "??????")
            };

            using (var message = new MailMessage("tranda96srk@gmail.com", toEmail)
            {
                Subject = "Public Transport - Ticket",
                Body = text
            })
            {
                try
                {
                    await smtpClient.SendMailAsync(message);
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"Error while sedding mail: {e.Message}");
                }
            }
        }
    }
}