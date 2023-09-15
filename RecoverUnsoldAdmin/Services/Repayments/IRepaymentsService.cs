using FluentPaginator.Lib.Page;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Services.Repayments;

public interface IRepaymentsService
{
    Task<Page<Repayment>> GetRepayments(RepaymentsFilter repaymentsFilter);
    Task MarkAsDone(Guid id, RepaymentValidationModel repaymentValidationModel);
}