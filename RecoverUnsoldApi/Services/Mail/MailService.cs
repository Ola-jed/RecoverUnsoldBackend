using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using RecoverUnsoldApi.Config;
using RecoverUnsoldApi.Services.Mail.Mailable;

namespace RecoverUnsoldApi.Services.Mail;

public class MailService: IMailService
{
    private readonly MailConfig _settings;

    public MailService(IOptions<MailConfig> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendEmailAsync(IMailable mailable)
    {
        var email = await mailable.Build();
        email.Sender = MailboxAddress.Parse(_settings.MailUser);
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_settings.MailUser, _settings.MailPassword);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}