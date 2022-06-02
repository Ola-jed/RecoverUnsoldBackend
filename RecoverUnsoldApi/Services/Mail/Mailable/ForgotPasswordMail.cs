using MimeKit;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

public class ForgotPasswordMail : IMailable
{
    public async Task<MimeMessage> Build()
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetHtmlBody()
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetPlainTextBody()
    {
        throw new NotImplementedException();
    }
}