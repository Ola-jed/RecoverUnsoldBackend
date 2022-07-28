using MimeKit;
using RecoverUnsoldApi.Services.Mail.Templates;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

public class OfferValidatedMail: IMailable
{
    private readonly string _name;
    private readonly string _destinationEmail;
    
    public OfferValidatedMail(string name, string destinationEmail)
    {
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
        return OfferValidatedMailTemplate.Html.Replace("{name}",_name);
    }

    private string GetPlainTextBody()
    {
        return OfferValidatedMailTemplate.Text.Replace("{name}",_name);
    }
}