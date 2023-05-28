namespace RecoverUnsoldDomain.Queue;

public sealed class MailMessage
{
    public required string Subject { get; set; }
    public required string Destination { get; set; }
    public required string HtmlBody { get; set; }
    public required string TextBody { get; set; }
}