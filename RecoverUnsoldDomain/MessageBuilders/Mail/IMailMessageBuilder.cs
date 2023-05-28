using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldDomain.MessageBuilders.Mail;

public interface IMailMessageBuilder
{
    public MailMessage BuildMailMessage();
}