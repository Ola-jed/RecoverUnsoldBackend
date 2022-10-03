using RecoverUnsoldDomain.Services.Mail.Mailable;

namespace RecoverUnsoldDomain.Services.Mail;

public class DummyMailService: IMailService
{
    public async Task TrySend(IMailable mailable)
    {
        await Task.CompletedTask;      
    }

    public async Task SendEmailAsync(IMailable mailable)
    {
        await Task.CompletedTask;
    }

    public void Queue(IMailable mailable)
    {
        // Nothing because it is just a dummy impl to avoid email sending
    }
}