namespace RecoverUnsoldDomain.Queue;

public sealed class MailMessage
{
    public string Subject { get; set; } = null!;
    public string Destination { get; set; } = null!;
    public string HtmlBody { get; set; } = null!;
    public string TextBody { get; set; } = null!;

    public MailPdfAttachment? PdfAttachment { get; set; }

    public sealed class MailPdfAttachment
    {
        public string FileName { get; set; } = null!;
        public string HtmlContent { get; set; } = null!;
    }
}