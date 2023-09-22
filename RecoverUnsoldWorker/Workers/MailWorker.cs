using System.Text;
using System.Text.Json;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using PuppeteerSharp;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RecoverUnsoldDomain.Config;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldWorker.Workers;

public class MailWorker : BackgroundService
{
    private readonly ILogger<MailWorker> _logger;
    private readonly MailConfig _mailConfig;
    private readonly RabbitmqConfig _rabbitmqConfig;
    private IModel? _channel;
    private IConnection? _connection;

    public MailWorker(ILogger<MailWorker> logger, IOptions<RabbitmqConfig> rabbitmqOptions,
        IOptions<MailConfig> mailOptions)
    {
        _logger = logger;
        _mailConfig = mailOptions.Value;
        _rabbitmqConfig = rabbitmqOptions.Value;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("The mail worker is starting");
        var connectionFactory = new ConnectionFactory
        {
            Uri = new Uri(_rabbitmqConfig.Uri),
            DispatchConsumersAsync = true
        };
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(
            QueueConstants.MailQueue,
            true,
            false,
            false,
            new Dictionary<string, object> { { "x-max-priority", QueueConstants.MaxPriority } }
        );

        return base.StartAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var mailConsumer = new AsyncEventingBasicConsumer(_channel);
        mailConsumer.Received += async (_, ea) =>
        {
            _logger.LogInformation("Got mail");
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            try
            {
                _logger.LogInformation("Processing a mail message");
                var mailMessage = JsonSerializer.Deserialize<MailMessage>(message);
                await SendMail(mailMessage!);
                _channel?.BasicAck(ea.DeliveryTag, false);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Json parse exception");
                _channel?.BasicNack(ea.DeliveryTag, false, false);
            }
            catch (AlreadyClosedException)
            {
                _logger.LogInformation("RabbitMQ is closed!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception was thrown");
            }
        };
        _channel.BasicConsume(QueueConstants.MailQueue, false, mailConsumer);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("The mail worker is stopping");
        await base.StopAsync(cancellationToken);
        _connection?.Close();
    }

    private async Task SendMail(MailMessage mailMessage)
    {
        var email = await BuildMimeMessage(mailMessage);
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_mailConfig.Host, _mailConfig.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_mailConfig.MailUser, _mailConfig.MailPassword);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }

    private async Task<MimeMessage> BuildMimeMessage(MailMessage mailMessage)
    {
        var email = new MimeMessage();
        email.Subject = mailMessage.Subject;
        email.To.Add(InternetAddress.Parse(mailMessage.Destination));
        email.Sender = MailboxAddress.Parse(_mailConfig.MailUser);
        email.From.Add(MailboxAddress.Parse(_mailConfig.MailUser));
        var builder = new BodyBuilder
        {
            HtmlBody = mailMessage.HtmlBody,
            TextBody = mailMessage.TextBody
        };

        if (mailMessage.PdfAttachment != null)
        {
            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(mailMessage.PdfAttachment.HtmlContent);
            var pdfData = await page.PdfDataAsync();
            builder.Attachments.Add(mailMessage.PdfAttachment.FileName, pdfData);
        }

        email.Body = builder.ToMessageBody();
        return email;
    }
}