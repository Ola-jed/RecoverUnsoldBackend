using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldDomain.Data;

namespace RecoverUnsoldApi.Services.Distributors;

public class DistributorsService : IDistributorsService
{
    private readonly DataContext _context;

    public DistributorsService(DataContext context)
    {
        _context = context;
    }

    public async Task<Page<DistributorInformationDto>> GetDistributors(PaginationParameter paginationParameter,
        string? name = null)
    {
        var distributorsSource = _context
            .Distributors
            .AsNoTracking()
            .Where(d => d.EmailVerifiedAt != null);
        if (name != null && name.Trim() != string.Empty)
        {
            distributorsSource = distributorsSource.Where(d => EF.Functions.Like(d.Username, $"%{name}%"));
        }

        var page = await distributorsSource
            .AsyncPaginate(paginationParameter, o => o.CreatedAt, PaginationOrder.Descending);

        return page.Map(d => d.ToDistributorInformationDto());
    }

    public async Task<IEnumerable<DistributorLabelReadDto>> GetDistributorsLabels()
    {
        return await _context.Distributors
            .AsNoTracking()
            .Where(d => d.EmailVerifiedAt != null)
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

    public async Task<bool> DistributorExists(Guid id)
    {
        return await _context.Distributors.AnyAsync(d => d.Id == id);
    }
}