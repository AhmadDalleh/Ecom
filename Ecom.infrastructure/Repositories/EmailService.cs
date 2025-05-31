using Ecom.Core.DTOs;
using Ecom.Core.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories
{
    public class EmailService : IEmailService
    {//SMTP 
        private readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendEmail(EmailDTO emailDTO)
        {
            MimeMessage message = new();

            message.From.Add(new MailboxAddress("My Ecom", configuration["EmailSetting:From"]));
            message.Subject = emailDTO.Subject;
            message.To.Add(new MailboxAddress(emailDTO.To, emailDTO.To));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailDTO.Content,
            };
            using(var smtp=new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync(
                        configuration["EmailSetting:Smtp"],
                        int.Parse(configuration["EmailSetting:Port"]), true);

                    await smtp.AuthenticateAsync(
                        configuration["EmailSetting:UserName"],
                        configuration["EmailSetting:Password"]);

                    await smtp.SendAsync(message);

                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    smtp.Disconnected(true);
                    smtp.Dispose();
                }
            }
        }
    }
}
