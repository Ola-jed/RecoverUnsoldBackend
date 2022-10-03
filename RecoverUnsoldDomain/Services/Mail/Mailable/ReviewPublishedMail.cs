using MimeKit;
using RecoverUnsoldDomain.Config;
using RecoverUnsoldDomain.Services.Mail.Templates;

namespace RecoverUnsoldDomain.Services.Mail.Mailable;

public class ReviewPublishedMail: IMailable
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
    
    public MimeMessage Build()
    {
        var email = new MimeMessage
        {
            Subject = "RecoverUnsold : Nouveau message",
            To = { MailboxAddress.Parse(_appOwner.Email) }
        };
        var builder = new BodyBuilder
        {
            HtmlBody = GetHtmlBody(),
            TextBody = GetPlainTextBody()
        };
        email.Body = builder.ToMessageBody();
        email.ReplyTo.Add(InternetAddress.Parse(_emailAddress));
        return email;
    }

    private string GetHtmlBody()
    {
        return ReviewPublishedMailTemplate.Html
            .Replace("{name}", _appOwner.Name)
            .Replace("{username}", _username)
            .Replace("{emailAddress}", _emailAddress)
            .Replace("{message}", _message);
    }

    private string GetPlainTextBody()
    {
        return ReviewPublishedMailTemplate.Text
            .Replace("{name}", _appOwner.Name)
            .Replace("{username}", _username)
            .Replace("{emailAddress}", _emailAddress)
            .Replace("{message}", _message);
    }
}