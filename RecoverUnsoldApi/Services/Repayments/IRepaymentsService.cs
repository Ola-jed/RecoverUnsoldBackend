using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Repayments;

public interface IRepaymentsService
{
    Task<Page<RepaymentReadDto>> GetRepayments(Guid userId, PaginationParameter paginationParameter,
        RepaymentFilterDto repaymentFilterDto);

    Task<RepaymentReadDto?> GetForUser(Guid id, Guid userId);
}