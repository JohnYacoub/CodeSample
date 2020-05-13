using Microsoft.Extensions.Options;
using Lynwood.Models.AppSettings;
using Lynwood.Models.Requests;
using Lynwood.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Lynwood.Services
{
    public class EmailService : IEmailService
    {
        private AppKeys _apiKey;

        public EmailService(IOptions<AppKeys> apiKey)
        {
            _apiKey = apiKey.Value;
        }

        public async Task Send(EmailSendRequest model)
        {
            SendGridClient client = new SendGridClient(_apiKey.SendGridAppKey.ToString());
            EmailAddress sender = new EmailAddress(model.From, model.Sender);
            EmailAddress recipient = new EmailAddress(model.To, model.Recipient);
            var msg = MailHelper.CreateSingleEmail(sender, recipient, model.Subject, model.PlainTextContent, model.HtmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        public async Task RegisterEmail(string recipient, Guid token)
        {
            EmailSendRequest emailModel = new EmailSendRequest();
            emailModel.From = "admin@lynwood.ca";
            emailModel.Sender = "admin";
            emailModel.Subject = "Email Confirmation";
            emailModel.To = recipient;
            emailModel.Recipient = recipient;
            emailModel.HtmlContent = @"<h4>Thank you for registering with Lynwood Pathways! Please click the link below to confirm your email</h4><a href=""https://localhost:3000/confirm/validate?token=" + token + @"""\>Confirm email here</a><br/>";
            emailModel.PlainTextContent = "Please click the link to confirm your email";
           await Send(emailModel);
        }
    }
    //check token in database, if not null, swal & redirect
    //update user status to confirmed
}
