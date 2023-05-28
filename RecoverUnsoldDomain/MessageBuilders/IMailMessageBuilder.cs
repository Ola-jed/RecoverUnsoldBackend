using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldDomain.MessageBuilders;

public interface IMailMessageBuilder
{
    public MailMessage BuildMailMessage();
}