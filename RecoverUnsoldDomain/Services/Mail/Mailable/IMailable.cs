using MimeKit;

namespace RecoverUnsoldDomain.Services.Mail.Mailable;

public interface IMailable
{
    MimeMessage Build();
}