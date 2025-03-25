using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Core.Handlers;

public interface ITransactionHandler
{
    Task<ResponseBase<Transaction?>> CreateAsync(CreateTransactionRequest request);
    Task<ResponseBase<Transaction?>> UpdateAsync(UpdateTransactionRequest request);
    Task<ResponseBase<Transaction?>> DeleteAsync(DeleteTransactionRequest request);
    Task<ResponseBase<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request);
    Task<PagedResponseBase<List<Transaction>?>> GetByPeriodAsync(GetTransactionByPeriodRequest request);
}