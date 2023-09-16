using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldDomain.Data;

namespace RecoverUnsoldApi.Services.Repayments;

public class RepaymentsService : IRepaymentsService
{
    private readonly DataContext _context;

    public RepaymentsService(DataContext context)
    {
        _context = context;
    }

    public async Task<Page<RepaymentReadDto>> GetRepayments(Guid userId, PaginationParameter paginationParameter,
        RepaymentFilterDto repaymentFilterDto)
    {
        var query = _context.Repayments
            .Include(r => r.Order)
            .ThenInclude(o => o!.Offer)
            .Where(r => r.Order!.Offer!.DistributorId == userId);

        var resultingQuery = repaymentFilterDto.Done switch
        {
            true => query.Where(r => r.Done),
            false => query.Where(r => !r.Done),
            _ => query
        };

        var page = await resultingQuery.AsyncPaginate(paginationParameter, r => r.CreatedAt);
        return page.Map(r => r.ToRepaymentReadDto());
    }

    public async Task<bool> BelongsToUser(Guid id, Guid userId)
    {
        return await _context.Repayments
            .Include(r => r.Order)
            .ThenInclude(o => o!.Offer)
            .AnyAsync(r => r.Order!.Offer!.DistributorId == userId);
    }

    public async Task<RepaymentReadDto?> GetForUser(Guid id, Guid userId)
    {
        var repayment = await _context.Repayments
            .Include(r => r.Order)
            .ThenInclude(o => o!.Offer)
            .Where(r => r.Order!.Offer!.DistributorId == userId)
            .FirstOrDefaultAsync(r => r.Id == id);

        return repayment?.ToRepaymentReadDto();
    }
}