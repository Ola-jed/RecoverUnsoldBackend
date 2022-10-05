using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldDomain.Dto;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Extensions;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldDomain.Services.Distributors;

public class DistributorsService: IDistributorsService
{
    private readonly DataContext _context;

    public DistributorsService(DataContext context)
    {
        _context = context;
    }

    public async Task<UrlPage<DistributorInformationDto>> GetDistributors(UrlPaginationParameter urlPaginationParameter,
        string? name = null)
    {
        var distributorsSource = _context
            .Distributors
            .AsNoTracking();
        if(name != null && name.Trim() != string.Empty)
        {
            distributorsSource = distributorsSource.Where(d => EF.Functions.Like(d.Username, $"%{name}%"));
        }

        return await Task.Run(() => distributorsSource
            .UrlPaginate(urlPaginationParameter, o => o.CreatedAt, PaginationOrder.Descending)
            .ToDistributorInformationReadDto()
        );
    }

    public async Task<UrlPage<Distributor>> ListDistributors(UrlPaginationParameter urlPaginationParameter, string? name = null)
    {
        var distributorsSource = _context
            .Distributors
            .AsNoTracking();
        if(name != null && name.Trim() != string.Empty)
        {
            distributorsSource = distributorsSource.Where(d => EF.Functions.Like(d.Username, $"%{name}%"));
        }

        return await Task.Run(() => distributorsSource
            .UrlPaginate(urlPaginationParameter, o => o.CreatedAt, PaginationOrder.Descending)
        );
    }

    public async Task<IEnumerable<DistributorLabelReadDto>> GetDistributorsLabels()
    {
        return await _context.Distributors
            .AsNoTracking()
            .Select(d => d.ToDistributorLabelReadDto())
            .ToListAsync();
    }

    public async Task<DistributorInformationDto?> GetDistributor(Guid id)
    {
        var distributor = await _context.Distributors
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);
        return distributor?.ToDistributorInformationDto();
    }
}