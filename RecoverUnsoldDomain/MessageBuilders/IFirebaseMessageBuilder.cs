using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldDomain.MessageBuilders;

public interface IFirebaseMessageBuilder
{
    public FirebaseMessage BuildFirebaseMessage();
}