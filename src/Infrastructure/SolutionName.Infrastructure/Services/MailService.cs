using Microsoft.Extensions.Configuration;
using SolutionName.Application.Abstractions.Services;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SolutionName.Infrastructure.Services
{
    public class MailService : IMailService
    {
        readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos)
                mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(_configuration["Mail:Username"], "Company Name", System.Text.Encoding.UTF8);

            SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Host = _configuration["Mail:Host"];
            await smtp.SendMailAsync(mail);
        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            StringBuilder mail = new();
            mail.AppendLine("Hello<br>If you have requested a new password, you can renew your password from the link below.<br><strong><a target=\"_blank\" href=\"");
            mail.AppendLine(_configuration["AngularClientUrl"]);
            mail.AppendLine("/update-password/");
            mail.AppendLine(userId);
            mail.AppendLine("/");
            mail.AppendLine(resetToken);
            mail.AppendLine("\">Click here for new password request...</a></strong><br><br><span style=\"font-size:12px;\">NOT : If this request has not been made by you, please do not take this e-mail seriously.</span><br>Regards...<br><br><br>Company Name");

            await SendMailAsync(to, "Password Reset Request", mail.ToString());
        }

        public async Task SendCompletedOrderMailAsync(string to, string orderCode, DateTime orderDate, string userName)
        {
            string mail = $"$Hello Mr {userName}<br>\" +\r\n Your order with code {orderCode}, which you placed on the date, has been completed and given to the cargo company.";

            await SendMailAsync(to, $" Your Order with {orderCode} Number Completed!", mail);

        }

    }

}
