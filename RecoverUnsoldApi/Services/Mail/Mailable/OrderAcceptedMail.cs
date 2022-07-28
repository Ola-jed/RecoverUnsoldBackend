using System.Globalization;
using MimeKit;
using RecoverUnsoldApi.Services.Mail.Templates;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

public class OrderAcceptedMail : IMailable
{
    private readonly string _name;
    private readonly DateTime _orderDate;
    private readonly decimal _offerAmount;
    private readonly DateTime _offerPublishDate;
    private readonly DateTime _withdrawalDate;
    private readonly string _destinationEmail;

    public OrderAcceptedMail(string name, DateTime orderDate, decimal offerAmount,
        DateTime offerPublishDate, DateTime withdrawalDate, string destinationEmail)
    {
        _name = name;
        _orderDate = orderDate;
        _offerAmount = offerAmount;
        _offerPublishDate = offerPublishDate;
        _withdrawalDate = withdrawalDate;
        _destinationEmail = destinationEmail;
    }

    public MimeMessage Build()
    {
        var email = new MimeMessage
        {
            Subject = "RecoverUnsold : Commande acceptée",
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
        return OrderAcceptedMailTemplate.Email
            .Replace("{name}", _name)
            .Replace("{orderDate}", _orderDate.ToShortDateString())
            .Replace("{offerAmount}", _offerAmount.ToString(CultureInfo.InvariantCulture))
            .Replace("{offerPublishDate}", _offerPublishDate.ToShortDateString())
            .Replace("{withdrawalDate}", _withdrawalDate.ToShortDateString());
    }

    private string GetPlainTextBody()
    {
        return OrderAcceptedMailTemplate.Text
            .Replace("{name}", _name)
            .Replace("{orderDate}", _orderDate.ToShortDateString())
            .Replace("{offerAmount}", _offerAmount.ToString(CultureInfo.InvariantCulture))
            .Replace("{offerPublishDate}", _offerPublishDate.ToShortDateString())
            .Replace("{withdrawalDate}", _withdrawalDate.ToShortDateString());
    }
}