using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Queue;

public interface IQueueService
{
    public void QueueMail(MailMessage mailMessage, byte priority);
    public void QueueFirebaseMessage(FirebaseMessage firebaseMessage, byte priority);
}