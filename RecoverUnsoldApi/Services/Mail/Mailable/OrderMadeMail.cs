using RecoverUnsoldApi.Services.Mail.Templates;
using RecoverUnsoldDomain.MessageBuilders;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

public class OrderMadeMail : IMailMessageBuilder
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

    public MailMessage BuildMailMessage()
    {
        return new MailMessage
        {
            Subject = "RecoverUnsold : Commande passée",
            Destination = _destinationEmail,
            HtmlBody = OrderMadeMailTemplate.Html
                .Replace("{name}", _name)
                .Replace("{publishDate}", _publishDate.ToShortDateString()),
            TextBody = OrderMadeMailTemplate.Text
                .Replace("{name}", _name)
                .Replace("{publishDate}", _publishDate.ToShortDateString())
        };
    }
}