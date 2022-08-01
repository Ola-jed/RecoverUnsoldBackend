using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Entities;

namespace RecoverUnsoldApi.Services.Reviews;

public interface IReviewsService
{
    Task<UrlPage<ReviewReadDto>> ListReviews(UrlPaginationParameter urlPaginationParameter);
    Task<Review> Publish(Guid userId, string comment);
    Task Delete(Guid id);
}