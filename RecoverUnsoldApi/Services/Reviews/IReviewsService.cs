using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Reviews;

public interface IReviewsService
{
    Task<UrlPage<ReviewReadDto>> ListReviews(UrlPaginationParameter urlPaginationParameter);
    Task PublishReview(Guid userId, string comment);
}