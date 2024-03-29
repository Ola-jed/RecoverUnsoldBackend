using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldApi.Services.Reviews;

public class ReviewsService : IReviewsService
{
    private readonly DataContext _context;

    public ReviewsService(DataContext context)
    {
        _context = context;
    }

    public async Task<Page<ReviewReadDto>> ListReviews(PaginationParameter paginationParameter)
    {
        var page = await _context.Reviews
            .AsNoTracking()
            .AsyncPaginate(paginationParameter, r => r.CreatedAt, PaginationOrder.Descending);

        return page.Map(r => r.ToReviewReadDto());
    }

    public async Task Delete(Guid id)
    {
        await _context.Reviews
            .Where(r => r.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<Review> Publish(Guid userId, string comment)
    {
        var reviewEntityEntry = _context.Reviews.Add(new Review
        {
            Comment = comment,
            UserId = userId
        });
        await _context.SaveChangesAsync();
        return reviewEntityEntry.Entity;
    }
}