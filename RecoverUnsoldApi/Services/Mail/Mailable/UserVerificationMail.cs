using RecoverUnsoldApi.Services.Mail.Templates;
using RecoverUnsoldDomain.MessageBuilders;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

public class UserVerificationMail : IMailMessageBuilder
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

    public MailMessage BuildMailMessage()
    {
        return new MailMessage
        {
            Subject = "RecoverUnsold : Vérification de compte",
            Destination = _destinationEmail,
            HtmlBody = UserVerificationMailTemplate.Html
                .Replace("{token}", _token)
                .Replace("{name}", _name),
            TextBody = UserVerificationMailTemplate.Text
                .Replace("{token}", _token)
                .Replace("{name}", _name)
        };
    }
}