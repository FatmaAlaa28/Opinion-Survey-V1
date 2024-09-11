using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Opinion_Survey.Confirm_Email
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587, // or 465 for SSL
                    Credentials = new NetworkCredential("adm6774969@gmail.com", "123@Admin"),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 20000
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("adm6774969@gmail.com"),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (SmtpException ex)
            {
                // Log the exception
                Console.WriteLine($"SMTP Error: {ex.Message}");
                throw; // Re-throw or handle as appropriate
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Re-throw or handle as appropriate
            }
        }

    }

}
