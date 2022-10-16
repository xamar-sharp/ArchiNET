using System;
using System.Threading.Tasks;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
namespace ArchiNET.Droid.Assets
{
    public interface IMediator
    {
        Task SendMail(string receiver, string title, string body);
    }
    public sealed class AnonymouslyMediator : IMediator
    {
        public async Task SendMail(string receiver, string title, string body)
        {
            MimeMessage msg = new MimeMessage() { From = { new MailboxAddress("Anonymously", "somebody@gmail.com") }, To = { new MailboxAddress("Guy", receiver) } };
            msg.Body = new TextPart()
            {
                Text = body
            };
            msg.Subject = title;
            using(SmtpClient client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("somebody@gmail.com", "12345678");
                await client.SendAsync(msg);
            }
        }
    }
}