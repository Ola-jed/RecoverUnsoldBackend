using MimeKit;
using RecoverUnsoldApi.Services.Mail.Templates;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

public class UserVerificationMail : IMailable
{
    private readonly string _name;
    private readonly string _token;
    private readonly string _destinationEmail;

    public UserVerificationMail(string name, string token, string destinationEmail)
    {
        _name = name;
        _token = token;
        _destinationEmail = destinationEmail;
    }

    public MimeMessage Build()
    {
        var email = new MimeMessage
        {
            Subject = "RecoverUnsold : Vérification de compte",
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
        return UserVerificationMailTemplate.Html
            .Replace("{token}", _token)
            .Replace("{name}", _name);
    }

    private string GetPlainTextBody()
    {
        return UserVerificationMailTemplate.Text
            .Replace("{token}", _token)
            .Replace("{name}", _name);
    }
}