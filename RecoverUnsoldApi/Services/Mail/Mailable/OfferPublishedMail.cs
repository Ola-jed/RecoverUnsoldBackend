using System.Globalization;
using RecoverUnsoldApi.Services.Mail.Templates;
using RecoverUnsoldDomain.MessageBuilders;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

public class OfferPublishedMail : IMailMessageBuilder
{
    private readonly decimal _price;
    private readonly DateTime _startDate;
    private readonly string _destinationEmail;

    public OfferPublishedMail(decimal price, DateTime startDate, string destinationEmail)
    {
        _price = price;
        _startDate = startDate;
        _destinationEmail = destinationEmail;
    }

    public MailMessage BuildMailMessage()
    {
        return new MailMessage
        {
            Destination = _destinationEmail,
            Subject = "RecoverUnsold : Nouvelle offre publiée",
            HtmlBody = OfferPublishedMailTemplate.Html
                .Replace("{price}", _price.ToString(CultureInfo.InvariantCulture))
                .Replace("{startDate}", _startDate.ToShortDateString()),
            TextBody = OfferPublishedMailTemplate.Text
                .Replace("{price}", _price.ToString(CultureInfo.InvariantCulture))
                .Replace("{startDate}", _startDate.ToShortDateString())
        };
    }
}