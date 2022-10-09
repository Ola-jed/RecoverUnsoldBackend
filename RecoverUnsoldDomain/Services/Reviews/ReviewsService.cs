using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Dto;
using RecoverUnsoldDomain.Entities;
using RecoverUnsoldDomain.Extensions;

namespace RecoverUnsoldDomain.Services.Reviews;

public class ReviewsService : IReviewsService
{
    private readonly DataContext _context;

    public ReviewsService(DataContext context)
    {
        _context = context;
    }

    public async Task<UrlPage<ReviewReadDto>> ListReviews(UrlPaginationParameter urlPaginationParameter)
    {
        return await Task.Run(() => _context.Reviews
            .AsNoTracking()
            .UrlPaginate(urlPaginationParameter, r => r.CreatedAt, PaginationOrder.Descending)
            .ToReviewReadDto()
        );
    }

    public async Task Delete(Guid id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
        {
            return;
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
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