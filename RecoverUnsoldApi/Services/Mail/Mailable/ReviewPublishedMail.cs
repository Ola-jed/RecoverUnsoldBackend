using RecoverUnsoldApi.Services.Mail.Templates;
using RecoverUnsoldDomain.Config;
using RecoverUnsoldDomain.MessageBuilders;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

public class ReviewPublishedMail : IMailMessageBuilder
{
    private readonly AppOwner _appOwner;
    private readonly string _username;
    private readonly string _emailAddress;
    private readonly string _message;

    public ReviewPublishedMail(AppOwner appOwner, string username, string emailAddress, string message)
    {
        _appOwner = appOwner;
        _username = username;
        _emailAddress = emailAddress;
        _message = message;
    }

    public MailMessage BuildMailMessage()
    {
        return new MailMessage
        {
            Subject = "RecoverUnsold : Nouveau message",
            Destination = _appOwner.Email,
            HtmlBody = ReviewPublishedMailTemplate.Html
                .Replace("{name}", _appOwner.Name)
                .Replace("{username}", _username)
                .Replace("{emailAddress}", _emailAddress)
                .Replace("{message}", _message),
            TextBody = ReviewPublishedMailTemplate.Text
                .Replace("{name}", _appOwner.Name)
                .Replace("{username}", _username)
                .Replace("{emailAddress}", _emailAddress)
                .Replace("{message}", _message)
        };
    }
}