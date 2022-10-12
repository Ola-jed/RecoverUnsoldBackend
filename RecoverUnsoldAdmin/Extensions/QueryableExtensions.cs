using RecoverUnsoldAdmin.Models;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<Offer> ApplyFilters(this IQueryable<Offer> self,OffersFilter offersFilter)
    {
        if (offersFilter.MinPrice != null)
        {
            self = self.Where(o => o.Price >= offersFilter.MinPrice);
        }

        if (offersFilter.MaxPrice != null)
        {
            self = self.Where(o => o.Price <= offersFilter.MaxPrice);
        }

        if (offersFilter.MinDate != null)
        {
            self = self.Where(o => o.StartDate >= offersFilter.MinDate);
        }
        
        if (offersFilter.MaxDate != null)
        {
            self = self.Where(o => o.StartDate <= offersFilter.MinDate);
        }

        var now = DateTime.Now;

        self = offersFilter.Active switch
        {
            true  => self.Where(o => o.StartDate.AddSeconds(o.Duration) > now),
            false => self.Where(o => o.StartDate.AddSeconds(o.Duration) <= now),
            _     => self
        };

        return self;
    }

}