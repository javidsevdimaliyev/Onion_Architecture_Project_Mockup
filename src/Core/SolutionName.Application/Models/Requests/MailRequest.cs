using Microsoft.AspNetCore.Http;

namespace SolutionName.Application.Models.Requests;

public class MailRequest
{
    public string To { get; set; }
    public string From { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public string Port { get; set; }
    public string Subject { get; set; }
    public string EmailTemplateName { get; set; }
    public string Url { get; set; }
    public string Body { get; set; }
    public string FullName { get; set; }
    public List<IFormFile> Attachments { get; set; }
}