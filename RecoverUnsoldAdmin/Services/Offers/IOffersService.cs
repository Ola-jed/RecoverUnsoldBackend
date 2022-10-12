using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Services.Offers;

public interface IOffersService
{
    Task<UrlPage<Offer>> ListOffers(OffersFilter offersFilter, UrlPaginationParameter paginationParameter);
}