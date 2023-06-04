using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Queue;

public interface IQueueService
{
    public void QueueMail(MailMessage mailMessage, byte priority = QueueConstants.PriorityMedium);
    public void QueueMails(IEnumerable<MailMessage> mailMessages, byte priority = QueueConstants.PriorityMedium);
    public void QueueFirebaseMessage(FirebaseMessage firebaseMessage, byte priority = QueueConstants.PriorityMedium);

    public void QueueFirebaseMessages(IEnumerable<FirebaseMessage> firebaseMessages,
        byte priority = QueueConstants.PriorityMedium);
}