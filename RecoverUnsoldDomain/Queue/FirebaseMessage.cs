namespace RecoverUnsoldDomain.Queue;

public sealed class FirebaseMessage
{
    public List<string> FcmTokens { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
}