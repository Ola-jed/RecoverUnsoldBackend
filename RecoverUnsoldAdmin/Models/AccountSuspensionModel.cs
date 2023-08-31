namespace RecoverUnsoldAdmin.Models;

public class AccountSuspensionModel
{
    public string Reason { get; set; } = null!;
    public DateTime? Date { get; set; }
    public DateTime? EndDate { get; set; }
}