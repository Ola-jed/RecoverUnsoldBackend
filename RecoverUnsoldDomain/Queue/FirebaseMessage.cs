namespace RecoverUnsoldDomain.Queue;

public sealed class FirebaseMessage
{
    public required List<string> FcmTokens { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
}