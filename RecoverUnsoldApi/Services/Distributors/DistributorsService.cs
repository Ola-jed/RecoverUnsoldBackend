using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Data;
using RecoverUnsoldApi.Extensions;
using Microsoft.EntityFrameworkCore;

namespace RecoverUnsoldApi.Services.Distributors;

public class DistributorsService: IDistributorsService
{
    private readonly DataContext _context;

    public DistributorsService(DataContext context)
    {
        _context = context;
    }

    public async Task<UrlPage<DistributorInformationDto>> GetDistributors(UrlPaginationParameter urlPaginationParameter)
    {
        return await Task.Run(() => _context
            .Distributors
            .AsNoTracking()
            .ToDistributorInformationReadDto()
            .UrlPaginate(urlPaginationParameter, o => o.CreatedAt, PaginationOrder.Descending)
        );
    }

    public async Task<DistributorInformationDto?> GetDistributor(Guid id)
    {
        var distributor = await _context.Distributors
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);
        return distributor?.ToDistributorInformationDto();
    }
}