using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RecoverUnsoldDomain.Config;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Queue;

public class QueueService : IQueueService
{
    private readonly ILogger<QueueService> _logger;
    private readonly RabbitmqConfig _rabbitmqConfig;

    public QueueService(ILogger<QueueService> logger, IOptions<RabbitmqConfig> options)
    {
        _logger = logger;
        _rabbitmqConfig = options.Value;
    }

    public void QueueMail(MailMessage mailMessage, byte priority = QueueConstants.PriorityMedium)
    {
        Queue(mailMessage, QueueConstants.MailQueue, priority, QueueConstants.MaxPriority);
    }

    public void QueueMails(IEnumerable<MailMessage> mailMessages, byte priority = QueueConstants.PriorityMedium)
    {
        foreach (var mailMessage in mailMessages)
        {
            QueueMail(mailMessage, priority);
        }
    }

    public void QueueFirebaseMessage(FirebaseMessage firebaseMessage, byte priority = QueueConstants.PriorityMedium)
    {
        Queue(firebaseMessage, QueueConstants.FirebaseQueue, priority, QueueConstants.MaxPriority);
    }

    public void QueueFirebaseMessages(IEnumerable<FirebaseMessage> firebaseMessages,
        byte priority = QueueConstants.PriorityMedium)
    {
        foreach (var firebaseMessage in firebaseMessages)
        {
            QueueFirebaseMessage(firebaseMessage, priority);
        }
    }

    private void Queue<T>(T value, string queueName, byte priority, byte maxPriority)
    {
        _logger.LogInformation("Queuing on {} with priority {}", queueName, priority);
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_rabbitmqConfig.Uri)
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(
            queueName,
            true,
            false,
            false,
            new Dictionary<string, object> { { "x-max-priority", maxPriority } }
        );

        var properties = channel.CreateBasicProperties();
        properties.Priority = priority;
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value));
        channel.BasicPublish(string.Empty, queueName, properties, body);
    }
}