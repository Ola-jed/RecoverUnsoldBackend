using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldDomain.Dto;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldDomain.Services.Reviews;

public interface IReviewsService
{
    Task<UrlPage<ReviewReadDto>> ListReviews(UrlPaginationParameter urlPaginationParameter);
    Task<Review> Publish(Guid userId, string comment);
    Task Delete(Guid id);
}