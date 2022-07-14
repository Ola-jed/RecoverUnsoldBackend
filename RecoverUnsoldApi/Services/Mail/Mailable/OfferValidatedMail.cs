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
        return await Task.Run(() => OfferValidatedMailTemplate.Html
            .Replace("{name}",_name)
        );
    }

    public async Task<string> GetPlainTextBody()
    {
        return await Task.Run(() => OfferValidatedMailTemplate.Text
            .Replace("{name}",_name)
        );
    }
}