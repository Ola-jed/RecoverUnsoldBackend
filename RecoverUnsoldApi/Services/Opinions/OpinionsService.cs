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

    public async Task<UrlPage<OpinionReadDto>> Get(Guid orderId, UrlPaginationParameter urlPaginationParameter)
    {
        return await Task.Run(() => _context.Opinions
            .AsNoTracking()
            .Where(o => o.OrderId == orderId)
            .UrlPaginate(urlPaginationParameter, o => o.CreatedAt, PaginationOrder.Descending)
            .ToOpinionReadDto()
        );
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
        var opinion = await _context.Opinions.FindAsync(id);
        if (opinion == null)
        {
            return;
        }

        opinion.Comment = opinionUpdateDto.Comment;
        _context.Opinions.Update(opinion);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var opinion = await _context.Opinions.FindAsync(id);
        if (opinion == null)
        {
            return;
        }

        _context.Opinions.Remove(opinion);
        await _context.SaveChangesAsync();
    }
}