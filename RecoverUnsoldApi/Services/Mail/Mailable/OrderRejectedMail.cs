using System.Globalization;
using RecoverUnsoldApi.Services.Mail.Templates;
using RecoverUnsoldDomain.MessageBuilders;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

public class OrderRejectedMail : IMailMessageBuilder
{
    private readonly string _name;
    private readonly DateTime _orderDate;
    private readonly decimal _offerAmount;
    private readonly DateTime _offerPublishDate;
    private readonly string _destinationEmail;

    public OrderRejectedMail(string name, DateTime orderDate, decimal offerAmount,
        DateTime offerPublishDate, string destinationEmail)
    {
        _name = name;
        _orderDate = orderDate;
        _offerAmount = offerAmount;
        _offerPublishDate = offerPublishDate;
        _destinationEmail = destinationEmail;
    }

    public MailMessage BuildMailMessage()
    {
        return new MailMessage
        {
            Subject = "RecoverUnsold : Commande refusée",
            Destination = _destinationEmail,
            HtmlBody = OrderRejectedMailTemplate.Html
                .Replace("{name}", _name)
                .Replace("{orderDate}", _orderDate.ToShortDateString())
                .Replace("{offerAmount}", _offerAmount.ToString(CultureInfo.InvariantCulture))
                .Replace("{offerPublishDate}", _offerPublishDate.ToShortDateString()),
            TextBody = OrderRejectedMailTemplate.Text
                .Replace("{name}", _name)
                .Replace("{orderDate}", _orderDate.ToShortDateString())
                .Replace("{offerAmount}", _offerAmount.ToString(CultureInfo.InvariantCulture))
                .Replace("{offerPublishDate}", _offerPublishDate.ToShortDateString())
        };
    }
}