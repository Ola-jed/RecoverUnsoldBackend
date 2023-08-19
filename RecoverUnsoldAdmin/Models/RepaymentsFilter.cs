namespace RecoverUnsoldAdmin.Models;

public class RepaymentsFilter
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public bool? Done { get; set; } = null;
    public string? Search { get; set; } = null;
}