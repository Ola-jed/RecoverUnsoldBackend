using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Services.Repayments;

public interface IRepaymentsService
{
    Task<Page<Repayment>> ListRepayments(PaginationParameter paginationParameter, bool? done = null);
}