using MimeKit;
using RecoverUnsoldApi.Entities;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

public class OrderMadeMail: IMailable
{
    private readonly Order _order;
    private readonly string _name;
    private readonly string _destinationEmail;

    public OrderMadeMail(Order order, string name, string destinationEmail)
    {
        _order = order;
        _destinationEmail = destinationEmail;
        _name = name;
    }
    
    public async Task<MimeMessage> Build()
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetHtmlBody()
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetPlainTextBody()
    {
        throw new NotImplementedException();
    }
}