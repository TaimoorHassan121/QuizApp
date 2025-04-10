using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace QuizTask.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderPassword = _configuration["EmailSettings:SenderPassword"];

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(toEmail);

                try
                {
                    await client.SendMailAsync(mailMessage);
                    Console.WriteLine("✅ Email sent successfully.");
                    return true;
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine($"❌ SMTP Error: {ex.StatusCode} - {ex.Message}");
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ General Error: {ex.Message}");
                    return false;
                }
            }
        }

    }
}
