using FluentPaginator.Lib.Page;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Services.Repayments;

public interface IRepaymentsService
{
    Task<Page<Repayment>> ListRepayments(RepaymentsFilter repaymentsFilter);
}