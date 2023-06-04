namespace RecoverUnsoldDomain.Queue;

public static class QueueConstants
{
    public const string MailQueue = "mail";
    public const string PdfQueue = "pdf";
    public const string FirebaseQueue = "firebase";
    
    public const byte MaxPriority = 10;
    public const byte PriorityLow = 1;
    public const byte PriorityMedium = 5;
    public const byte PriorityHigh = 10;
}