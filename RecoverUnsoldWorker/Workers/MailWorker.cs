using System.Text;
using System.Text.Json;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
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
            HostName = _rabbitmqConfig.HostName,
            Port = _rabbitmqConfig.Port,
            Password = _rabbitmqConfig.Password,
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
                _logger.LogInformation("Processing a mail message : {Message}", message);
                var mailMessage = JsonSerializer.Deserialize<MailMessage>(message);
                await SendMail(mailMessage!);
                _channel?.BasicAck(ea.DeliveryTag, false);
            }
            catch (JsonException e)
            {
                _logger.LogError("Json parse exception : {Error}", e);
                _channel?.BasicNack(ea.DeliveryTag, false, false);
            }
            catch (AlreadyClosedException)
            {
                _logger.LogInformation("RabbitMQ is closed!");
            }
            catch (Exception e)
            {
                _logger.LogError(default, e, "{Message}", e.Message);
            }
        };

        _channel.BasicConsume(QueueConstants.MailQueue, false, mailConsumer);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        _connection?.Close();
        _logger.LogInformation("The mail worker is stopping");
    }

    private async Task SendMail(MailMessage mailMessage)
    {
        // TODO : Handle pdf attachments
        // We will use IronPDF https://ironpdf.com/
        var email = new MimeMessage();
        email.Subject = mailMessage.Subject;
        email.To.Add(InternetAddress.Parse(mailMessage.Destination));
        var builder = new BodyBuilder
        {
            HtmlBody = mailMessage.HtmlBody,
            TextBody = mailMessage.TextBody
        };
        email.Body = builder.ToMessageBody();
        email.Sender = MailboxAddress.Parse(_mailConfig.MailUser);
        email.From.Add(MailboxAddress.Parse(_mailConfig.MailUser));
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_mailConfig.Host, _mailConfig.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_mailConfig.MailUser, _mailConfig.MailPassword);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}