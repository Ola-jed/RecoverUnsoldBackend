using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldDomain.Entities;
using RecoverUnsoldApi.Extensions;

namespace RecoverUnsoldApi.Services.Opinions;

public class OpinionsService : IOpinionsService
{
    private readonly DataContext _context;

    public OpinionsService(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> IsUserAuthor(Guid userId, Guid opinionId)
    {
        return await _context.Opinions
            .Include(o => o.Order)
            .AnyAsync(o => o.Order != null && o.Id == opinionId && o.Order.CustomerId == userId);
    }

    public async Task<Page<OpinionReadDto>> Get(Guid orderId, PaginationParameter paginationParameter)
    {
        var page = await _context.Opinions
            .AsNoTracking()
            .Where(o => o.OrderId == orderId)
            .AsyncPaginate(paginationParameter, o => o.CreatedAt, PaginationOrder.Descending);
        
        return page.ToOpinionReadDto();
    }

    public async Task<OpinionReadDto?> Get(Guid id)
    {
        var opinion = await _context.Opinions
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);
        return opinion?.ToOpinionReadDto();
    }

    public async Task<OpinionReadDto> Publish(OpinionCreateDto opinionCreateDto, Guid orderId)
    {
        var opinionEntityEntry = _context.Opinions.Add(new Opinion
        {
            Comment = opinionCreateDto.Comment,
            OrderId = orderId
        });
        await _context.SaveChangesAsync();
        return opinionEntityEntry.Entity.ToOpinionReadDto();
    }

    public async Task Update(Guid id, OpinionUpdateDto opinionUpdateDto)
    {
        await _context.Opinions
            .Where(o => o.Id == id)
            .ExecuteUpdateAsync(opinion => opinion.SetProperty(x => x.Comment, opinionUpdateDto.Comment));
    }

    public async Task Delete(Guid id)
    {
        await _context.Opinions
            .Where(o => o.Id == id)
            .ExecuteDeleteAsync();
    }
}