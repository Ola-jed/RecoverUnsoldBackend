using System.Globalization;
using MimeKit;
using RecoverUnsoldApi.Services.Mail.Templates;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

public class OfferPublishedMail: IMailable
{
    private readonly decimal _price;
    private readonly DateTime _startDate;
    private readonly IEnumerable<string> _destinationEmails;

    public OfferPublishedMail(decimal price, DateTime startDate,
        IEnumerable<string> destinationEmails)
    {
        _price = price;
        _startDate = startDate;
        _destinationEmails = destinationEmails;
    }

    public MimeMessage Build()
    {
        var email = new MimeMessage();
        email.Subject = "RecoverUnsold : Nouvelle offre publiée";
        email.To.AddRange(_destinationEmails.Select(InternetAddress.Parse));
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
        return OfferPublishedMailTemplate.Html
            .Replace("{price}", _price.ToString(CultureInfo.InvariantCulture))
            .Replace("{startDate}", _startDate.ToShortDateString());
    }

    private string GetPlainTextBody()
    {
        return OfferPublishedMailTemplate.Text
            .Replace("{price}", _price.ToString(CultureInfo.InvariantCulture))
            .Replace("{startDate}", _startDate.ToShortDateString());
    }
}