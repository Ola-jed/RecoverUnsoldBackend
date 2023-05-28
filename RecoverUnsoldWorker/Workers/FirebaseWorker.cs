using System.Collections.Immutable;
using System.Text;
using System.Text.Json;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RecoverUnsoldDomain.Config;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldWorker.Workers;

public class FirebaseWorker: BackgroundService
{
    private readonly ILogger<FirebaseWorker> _logger;
    private readonly RabbitmqConfig _rabbitmqConfig;
    private IModel? _channel;
    private IConnection? _connection;
    
    public FirebaseWorker(ILogger<FirebaseWorker> logger, IOptions<RabbitmqConfig> rabbitmqOptions)
    {
        _logger = logger;
        _rabbitmqConfig = rabbitmqOptions.Value;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("The firebase worker is starting");
        var connectionFactory = new ConnectionFactory
        {
            HostName = _rabbitmqConfig.HostName,
            Port = _rabbitmqConfig.Port,
            Password = _rabbitmqConfig.Password,
            UserName = _rabbitmqConfig.UserName,
            DispatchConsumersAsync = true
        };
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(
            QueueConstants.FirebaseQueue,
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
            _logger.LogInformation("Got firebase message");
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            try
            {
                _logger.LogInformation("Processing a firebase message : {Message}", message);
                var firebaseMessage = JsonSerializer.Deserialize<FirebaseMessage>(message);
                await SendNotification(firebaseMessage!);
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
                _logger.LogError(default, e,"{Message}", e.Message);
            }
        };

        _channel.BasicConsume(QueueConstants.MailQueue, false, mailConsumer);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        _connection?.Close();
        _logger.LogInformation("The firebase worker is stopping");
    }

    private async Task SendNotification(FirebaseMessage firebaseMessage)
    {
        var fcmMessages = firebaseMessage.FcmTokens
            .Select(token => new Message
            {
                Token = token,
                Notification = new Notification
                {
                    Title = firebaseMessage.Title,
                    Body = firebaseMessage.Body
                }
            }).ToArray();

        if (fcmMessages.Length == 0)
        {
            return;
        }
        
        try
        {
            var result = await FirebaseMessaging.DefaultInstance.SendAllAsync(fcmMessages);
            foreach (var resultResponse in result?.Responses ?? ImmutableList<SendResponse>.Empty)
            {
                if (resultResponse.IsSuccess)
                {
                    _logger.LogInformation("Successfully sent message to: {MessageId}", resultResponse.MessageId);
                }
                else
                {
                    _logger.LogError("Error sending message to: {MessageId}\n Error : {ErrorMessage}",
                        resultResponse.MessageId, resultResponse.Exception.Message);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Notification sending failed : {}", e.Message);
        }
    }
}