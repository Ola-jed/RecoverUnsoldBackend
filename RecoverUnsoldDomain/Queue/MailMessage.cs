namespace RecoverUnsoldDomain.Queue;

public sealed class MailMessage
{
    public required string Subject { get; set; }
    public required string Destination { get; set; }
    public required string HtmlBody { get; set; }
    public required string TextBody { get; set; }

    public MailPdfAttachment? PdfAttachment { get; set; }

    public sealed class MailPdfAttachment
    {
        public required string FileName { get; set; }
        public required string HtmlContent { get; set; }
    }
}