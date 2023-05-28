using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldDomain.MessageBuilders.Firebase;

public interface IFirebaseMessageBuilder
{
    public FirebaseMessage BuildFirebaseMessage();
}