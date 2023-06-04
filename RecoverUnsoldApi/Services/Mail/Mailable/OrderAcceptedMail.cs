using System.Globalization;
using RecoverUnsoldApi.Services.Mail.Templates;
using RecoverUnsoldDomain.MessageBuilders;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

public class OrderAcceptedMail : IMailMessageBuilder
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

    public MailMessage BuildMailMessage()
    {
        return new MailMessage
        {
            Subject = "RecoverUnsold : Commande acceptée",
            Destination = _destinationEmail,
            HtmlBody = OrderAcceptedMailTemplate.Html
                .Replace("{name}", _name)
                .Replace("{orderDate}", _orderDate.ToShortDateString())
                .Replace("{offerAmount}", _offerAmount.ToString(CultureInfo.InvariantCulture))
                .Replace("{offerPublishDate}", _offerPublishDate.ToShortDateString())
                .Replace("{withdrawalDate}", _withdrawalDate.ToShortDateString()),
            TextBody = OrderAcceptedMailTemplate.Text
                .Replace("{name}", _name)
                .Replace("{orderDate}", _orderDate.ToShortDateString())
                .Replace("{offerAmount}", _offerAmount.ToString(CultureInfo.InvariantCulture))
                .Replace("{offerPublishDate}", _offerPublishDate.ToShortDateString())
                .Replace("{withdrawalDate}", _withdrawalDate.ToShortDateString())
        };
    }
}