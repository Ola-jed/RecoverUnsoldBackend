using RecoverUnsoldApi.Services.Mail.Templates;
using RecoverUnsoldDomain.MessageBuilders;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

public class OfferValidatedMail : IMailMessageBuilder
{
    private readonly string _name;
    private readonly string _destinationEmail;

    public OfferValidatedMail(string name, string destinationEmail)
    {
        _destinationEmail = destinationEmail;
        _name = name;
    }

    public MailMessage BuildMailMessage()
    {
        return new MailMessage
        {
            Subject = "RecoverUnsold : Commande validée",
            Destination = _destinationEmail,
            HtmlBody = OfferValidatedMailTemplate.Html.Replace("{name}", _name),
            TextBody = OfferValidatedMailTemplate.Text.Replace("{name}", _name)
        };
    }
}