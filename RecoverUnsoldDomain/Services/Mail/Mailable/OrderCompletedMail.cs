using System.Globalization;
using MimeKit;
using RecoverUnsoldDomain.Services.Mail.Templates;

namespace RecoverUnsoldDomain.Services.Mail.Mailable;

public class OrderCompletedMail : IMailable
{
    private readonly string _name;
    private readonly DateTime _orderDate;
    private readonly decimal _offerAmount;
    private readonly DateTime _offerPublishDate;
    private readonly string _destinationEmail;

    public OrderCompletedMail(string name, DateTime orderDate, decimal offerAmount,
        DateTime offerPublishDate, string destinationEmail)
    {
        _name = name;
        _orderDate = orderDate;
        _offerAmount = offerAmount;
        _offerPublishDate = offerPublishDate;
        _destinationEmail = destinationEmail;
    }

    public MimeMessage Build()
    {
        var email = new MimeMessage
        {
            Subject = "RecoverUnsold : Commande finalisée",
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
        return OrderCompletedMailTemplate.Html
            .Replace("{name}", _name)
            .Replace("{orderDate}", _orderDate.ToShortDateString())
            .Replace("{offerAmount}", _offerAmount.ToString(CultureInfo.InvariantCulture))
            .Replace("{offerPublishDate}", _offerPublishDate.ToShortDateString());
    }

    private string GetPlainTextBody()
    {
        return OrderCompletedMailTemplate.Text
            .Replace("{name}", _name)
            .Replace("{orderDate}", _orderDate.ToShortDateString())
            .Replace("{offerAmount}", _offerAmount.ToString(CultureInfo.InvariantCulture))
            .Replace("{offerPublishDate}", _offerPublishDate.ToShortDateString());
    }
}