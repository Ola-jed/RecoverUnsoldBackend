using MimeKit;
using RecoverUnsoldDomain.Services.Mail.Templates;

namespace RecoverUnsoldDomain.Services.Mail.Mailable;

public class OrderMadeMail : IMailable
{
    private readonly DateTime _publishDate;
    private readonly string _name;
    private readonly string _destinationEmail;

    public OrderMadeMail(DateTime publishDate, string name, string destinationEmail)
    {
        _publishDate = publishDate;
        _destinationEmail = destinationEmail;
        _name = name;
    }

    public MimeMessage Build()
    {
        var email = new MimeMessage
        {
            Subject = "RecoverUnsold : Commande passée",
            To = { MailboxAddress.Parse(_destinationEmail) }
        };
        var builder = new BodyBuilder
        {
            HtmlBody = GetHtmlBody(),
            TextBody = GetPlainTextBody()
        };
        email.Body = builder.ToMessageBody();
        return email;
    }

    private string GetHtmlBody()
    {
        return OrderMadeMailTemplate.Html
            .Replace("{name}", _name)
            .Replace("{publishDate}", _publishDate.ToShortDateString());
    }

    private string GetPlainTextBody()
    {
        return OrderMadeMailTemplate.Text
            .Replace("{name}", _name)
            .Replace("{publishDate}", _publishDate.ToShortDateString());
    }
}