using RecoverUnsoldDomain.Services.Mail.Mailable;

namespace RecoverUnsoldDomain.Services.Mail;

public interface IMailService
{
    Task TrySend(IMailable mailable);
    Task SendEmailAsync(IMailable mailable);
    void Queue(IMailable mailable);
}