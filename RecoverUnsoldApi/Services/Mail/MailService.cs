using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using RecoverUnsoldDomain.Config;
using RecoverUnsoldDomain.Infrastructure;
using RecoverUnsoldApi.Services.Mail.Mailable;

namespace RecoverUnsoldApi.Services.Mail;

public class MailService : IMailService
{
    private readonly MailConfig _settings;
    private readonly BackgroundWorkerQueue _backgroundWorkerQueue;
    private readonly ILogger<MailService> _logger;

    public MailService(IOptions<MailConfig> settings, BackgroundWorkerQueue backgroundWorkerQueue,
        ILogger<MailService> logger)
    {
        _backgroundWorkerQueue = backgroundWorkerQueue;
        _logger = logger;
        _settings = settings.Value;
    }

    public async Task TrySend(IMailable mailable)
    {
        try
        {
            await SendEmailAsync(mailable);
        }
        catch (Exception e)
        {
            _logger.LogError("Mail sending failed with exception {Message}", e.Message);
            Queue(mailable);
        }
    }

    public async Task SendEmailAsync(IMailable mailable)
    {
        var email = mailable.Build();
        email.Sender = MailboxAddress.Parse(_settings.MailUser);
        email.From.Add(MailboxAddress.Parse(_settings.MailUser));
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_settings.MailUser, _settings.MailPassword);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }

    public void Queue(IMailable mailable)
    {
        _backgroundWorkerQueue.QueueBackgroundWorkItem(async _ => await SendEmailAsync(mailable));
    }
}