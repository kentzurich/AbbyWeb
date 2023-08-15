using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BulkyBook.Utility
{
    public class EmailSender : IEmailSender
    {
        //public string SendGridSecretKey { get; set; }
        //public EmailSender(IConfiguration _config)
        //{
        //    SendGridSecretKey = _config.GetValue<string>("SendGrid:SecretKey");
        //}

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse("hello@domain.com"));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

            //send email
            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("kathsss16@gmail.com", "ztzjvcwhezphmvey");
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
            }

            return Task.CompletedTask;

            //var client = new SendGridClient(SendGridSecretKey);
            //// wala akong natanggap email. need yata domain na email talaga. temp email lang kasi gamit ko caligo5616@jobbrett.com
            //// pang domain emails lang to
            //var from = new EmailAddress("caligo5616@jobbrett.com", "Bulky Book"); 
            //var to = new EmailAddress(email);
            //var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            //return client.SendEmailAsync(message);
        }
    }
}
