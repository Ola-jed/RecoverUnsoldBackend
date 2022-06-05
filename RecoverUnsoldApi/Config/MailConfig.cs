namespace RecoverUnsoldApi.Config;

public class MailConfig
{
    public string MailUser { get; init; } = null!;
    public string MailDisplayName { get; init; } = null!;
    public string MailPassword { get; init; } = null!;
    public string Host { get; init; } = null!;
    public int Port { get; init; }
}