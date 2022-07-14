using MimeKit;
using RecoverUnsoldApi.Services.Mail.Templates;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

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

    public async Task<MimeMessage> Build()
    {
        var email = new MimeMessage
        {
            Subject = "RecoverUnsold : Commande passée",
            To = { MailboxAddress.Parse(_destinationEmail) }
        };
        var builder = new BodyBuilder
        {
            HtmlBody = await GetHtmlBody(),
            TextBody = await GetPlainTextBody()
        };
        email.Body = builder.ToMessageBody();
        return email;
    }

    public async Task<string> GetHtmlBody()
    {
        return await Task.Run(() => OrderMadeMailTemplate.Html
            .Replace("{name}", _name)
            .Replace("{publishDate}", _publishDate.ToShortDateString())
        );
    }

    public async Task<string> GetPlainTextBody()
    {
        return await Task.Run(() => OrderMadeMailTemplate.Text
            .Replace("{name}", _name)
            .Replace("{publishDate}", _publishDate.ToShortDateString())
        );
    }
}