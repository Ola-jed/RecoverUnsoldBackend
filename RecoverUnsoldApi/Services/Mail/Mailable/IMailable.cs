using MimeKit;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

public interface IMailable
{
    MimeMessage Build();
}