using FluentPaginator.Lib.Page;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Services.Offers;

public interface IOffersService
{
    Task<Page<Offer>> ListOffers(OffersFilter offersFilter);
}