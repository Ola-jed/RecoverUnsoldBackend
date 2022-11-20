using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldApi.Services.Reviews;

public interface IReviewsService
{
    Task<Page<ReviewReadDto>> ListReviews(PaginationParameter paginationParameter);
    Task<Review> Publish(Guid userId, string comment);
    Task Delete(Guid id);
}