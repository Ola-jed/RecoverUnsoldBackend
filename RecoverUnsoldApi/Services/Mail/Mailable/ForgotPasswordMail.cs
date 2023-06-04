using RecoverUnsoldApi.Services.Mail.Templates;
using RecoverUnsoldDomain.MessageBuilders;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

public class ForgotPasswordMail : IMailMessageBuilder
{
    private readonly string _name;
    private readonly string _token;
    private readonly string _destinationEmail;

    public ForgotPasswordMail(string name, string token, string destinationEmail)
    {
        _name = name;
        _token = token;
        _destinationEmail = destinationEmail;
    }

    public MailMessage BuildMailMessage()
    {
        return new MailMessage
        {
            Subject = "RecoverUnsold : Mot de passe oublié",
            Destination = _destinationEmail,
            HtmlBody = ForgotPasswordMailTemplate.Html
                .Replace("{token}", _token)
                .Replace("{name}", _name),
            TextBody = ForgotPasswordMailTemplate.Text
                .Replace("{token}", _token)
                .Replace("{name}", _name)
        };
    }
}