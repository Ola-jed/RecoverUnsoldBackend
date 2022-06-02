using MimeKit;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

public interface IMailable
{
    Task<MimeMessage> Build();
    Task<string> GetHtmlBody();
    Task<string> GetPlainTextBody();
}