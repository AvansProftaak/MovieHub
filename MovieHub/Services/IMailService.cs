using MovieHub.Models;

namespace MovieHub.Services;

public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}