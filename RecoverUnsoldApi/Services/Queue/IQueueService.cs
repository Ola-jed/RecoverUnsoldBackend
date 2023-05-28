using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Queue;

public interface IQueueService
{
    public void QueueMail(MailMessage mailMessage, byte priority);
    public void QueueMails(IEnumerable<MailMessage> mailMessages, byte priority);
    public void QueueFirebaseMessage(FirebaseMessage firebaseMessage, byte priority);
    public void QueueFirebaseMessages(IEnumerable<FirebaseMessage> firebaseMessages, byte priority);
}