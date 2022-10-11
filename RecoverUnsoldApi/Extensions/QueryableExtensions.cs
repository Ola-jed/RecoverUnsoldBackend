using RecoverUnsoldApi.Dto;
using RecoverUnsoldDomain.Entities;
using RecoverUnsoldDomain.Entities.Enums;

namespace RecoverUnsoldApi.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<Offer> ApplyFilters(this IQueryable<Offer> self,OfferFilterDto offerFilterDto)
    {
        if (offerFilterDto.MinPrice != null)
        {
            self = self.Where(o => o.Price >= offerFilterDto.MinPrice);
        }

        if (offerFilterDto.MaxPrice != null)
        {
            self = self.Where(o => o.Price <= offerFilterDto.MaxPrice);
        }

        if (offerFilterDto.MinDate != null)
        {
            self = self.Where(o => o.StartDate >= offerFilterDto.MinDate);
        }
        
        if (offerFilterDto.MaxDate != null)
        {
            self = self.Where(o => o.StartDate <= offerFilterDto.MinDate);
        }

        var now = DateTime.Now;

        self = offerFilterDto.Active switch
        {
            true  => self.Where(o => o.StartDate.AddSeconds(o.Duration) > now),
            false => self.Where(o => o.StartDate.AddSeconds(o.Duration) <= now),
            _     => self
        };

        return self;
    }

    public static IQueryable<Order> ApplyFilters(this IQueryable<Order> self, OrderFilterDto orderFilterDto)
    {
        return orderFilterDto.Status?.ToLowerInvariant() switch
        {
            "pending"   => self.Where(o => o.Status == Status.Pending),
            "approved"  => self.Where(o => o.Status == Status.Approved),
            "rejected"  => self.Where(o => o.Status == Status.Rejected),
            "completed" => self.Where(o => o.Status == Status.Completed),
            _           => self
        };
    }
}