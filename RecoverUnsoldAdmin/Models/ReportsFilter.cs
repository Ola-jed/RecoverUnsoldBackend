namespace RecoverUnsoldAdmin.Models;

public class ReportsFilter
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public bool? Processed { get; set; } = null;
    public string? Search { get; set; } = null;
}