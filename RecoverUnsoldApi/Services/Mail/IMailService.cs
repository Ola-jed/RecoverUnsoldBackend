using RecoverUnsoldApi.Services.Mail.Mailable;

namespace RecoverUnsoldApi.Services.Mail;

public interface IMailService
{
    Task SendEmailAsync(IMailable mailable);
    void Queue(IMailable mailable);
}