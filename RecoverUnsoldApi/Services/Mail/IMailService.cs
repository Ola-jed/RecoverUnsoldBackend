using RecoverUnsoldApi.Services.Mail.Mailable;

namespace RecoverUnsoldApi.Services.Mail;

public interface IMailService
{
    Task TrySend(IMailable mailable);
    Task SendEmailAsync(IMailable mailable);
    void Queue(IMailable mailable);
}