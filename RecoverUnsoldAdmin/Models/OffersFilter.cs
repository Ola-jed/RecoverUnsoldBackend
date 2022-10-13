namespace RecoverUnsoldAdmin.Models;

public class OffersFilter
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
    public bool? Active { get; set; }
}