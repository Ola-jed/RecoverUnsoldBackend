using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Queue;

public interface IQueueService
{
    void QueueMail(MailMessage mailMessage, byte priority = QueueConstants.PriorityMedium);
    void QueueMails(IEnumerable<MailMessage> mailMessages, byte priority = QueueConstants.PriorityMedium);
    void QueueFirebaseMessage(FirebaseMessage firebaseMessage, byte priority = QueueConstants.PriorityMedium);

    void QueueFirebaseMessages(IEnumerable<FirebaseMessage> firebaseMessages,
        byte priority = QueueConstants.PriorityMedium);
}