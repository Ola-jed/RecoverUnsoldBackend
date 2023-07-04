using RazorLight;
using RecoverUnsoldApi.Services.Mail.Templates;
using RecoverUnsoldDomain.Entities;
using RecoverUnsoldDomain.MessageBuilders;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Mail.Mailable;

public class InvoiceMail : IMailMessageBuilder
{
    private readonly Order _order;
    private readonly string _destinationEmail;
    private readonly string _name;
    private string _html = "";

    public InvoiceMail(Order order, string destinationEmail, string name)
    {
        _order = order;
        _destinationEmail = destinationEmail;
        _name = name;
    }

    public async Task GenerateHtml()
    {
        var invoiceNumber = _order.Number.ToString();
        var createdDate = _order.CreatedAt.ToShortDateString();
        var model = new
        {
            CompanyLogoUrl = "",
            InvoiceNumber = invoiceNumber,
            CreatedDate = createdDate,
            Amount = _order.Offer?.Price,
            Distributor = new
            {
                Name = _order.Offer?.Distributor?.Username,
                _order.Offer?.Distributor?.Phone,
                _order.Offer?.Distributor?.Email
            },
            Customer = new
            {
                Name = _order.Customer?.Username,
                _order.Customer?.Email
            },
            Products = _order.Offer?.Products.Select(p => new
            {
                p.Name,
                p.Description
            })
        };

        var engine = new RazorLightEngineBuilder()
            .EnableDebugMode()
            .UseEmbeddedResourcesProject(typeof(Program).Assembly, "RecoverUnsoldApi.Resource")
            .UseMemoryCachingProvider()
            .Build();
        _html = await engine.CompileRenderAsync("Invoice", model);
    }

    public MailMessage BuildMailMessage()
    {
        var invoiceNumber = _order.Id.ToString().Replace("-", "");
        var createdDate = _order.CreatedAt.ToShortDateString();

        return new MailMessage
        {
            Subject = "RecoverUnsold : Facture",
            Destination = _destinationEmail,
            HtmlBody = InvoiceMailTemplate.Html
                .Replace("{invoiceNumber}", invoiceNumber)
                .Replace("{name}", _name)
                .Replace("{date}", createdDate),
            TextBody = InvoiceMailTemplate.Text
                .Replace("{invoiceNumber}", invoiceNumber)
                .Replace("{name}", _name)
                .Replace("{date}", createdDate),
            PdfAttachment = new MailMessage.MailPdfAttachment
            {
                FileName = $"Facture {invoiceNumber}.pdf",
                HtmlContent = _html
            }
        };
    }
}