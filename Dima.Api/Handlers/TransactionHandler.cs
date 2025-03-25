using Dima.Api.Data;
using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class TransactionHandler(AppDbContext context) : ITransactionHandler
{
    public async Task<ResponseBase<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        try
        {
            var transaction = new Transaction
            {
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.Now,
                Amount = request.Amount,
                PaidOrReceivedAt = request.PaidOrReceivedAt,
                Title = request.Title,
                Type = request.Type
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            return new ResponseBase<Transaction?>(transaction, 201, "Transação criada com sucesso!");
        }
        catch
        {
            return new ResponseBase<Transaction?>(null, 500, "Não foi possível criar sua transação");
        }
    }

    public async Task<ResponseBase<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        try
        {
            var transaction = await context
                .Transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id );

            if (transaction is null)
                return new ResponseBase<Transaction?>(null, 404, "Transação não encontrada");

            transaction.CategoryId = request.CategoryId;
            transaction.Amount = request.Amount;
            transaction.Title = request.Title;
            transaction.Type = request.Type;
            transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;

            context.Transactions.Update(transaction);
            await context.SaveChangesAsync();

            return new ResponseBase<Transaction?>(transaction);
        }
        catch
        {
            return new ResponseBase<Transaction?>(null, 500, "Não foi possível recuperar sua transação");
        }
    }

    public async Task<ResponseBase<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await context
                .Transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id );

            if (transaction is null)
                return new ResponseBase<Transaction?>(null, 404, "Transação não encontrada");

            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();

            return new ResponseBase<Transaction?>(transaction);
        }
        catch
        {
            return new ResponseBase<Transaction?>(null, 500, "Não foi possível recuperar sua transação");
        }
    }

    public async Task<ResponseBase<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            var transaction = await context
                .Transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id );

            return transaction is null
                ? new ResponseBase<Transaction?>(null, 404, "Transação não encontrada")
                : new ResponseBase<Transaction?>(transaction);
        }
        catch
        {
            return new ResponseBase<Transaction?>(null, 500, "Não foi possível recuperar sua transação");
        }
    }

    public async Task<PagedResponseBase<List<Transaction>?>> GetByPeriodAsync(GetTransactionByPeriodRequest request)
    {
        try
        {
            request.StartDate ??= DateTime.Now.GetFirstDay();
            request.EndDate ??= DateTime.Now.GetLastDay();
        }
        catch
        {
            return new PagedResponseBase<List<Transaction>?>(null, 500,
                "Não foi possível determinar a data de início ou término");
        }

        try
        {
            var query = context
                .Transactions
                .AsNoTracking()
                .Where(x =>
                    x.PaidOrReceivedAt >= request.StartDate &&
                    x.PaidOrReceivedAt <= request.EndDate )
                .OrderBy(x => x.PaidOrReceivedAt);

            var transactions = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponseBase<List<Transaction>?>(
                transactions,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new PagedResponseBase<List<Transaction>?>(null, 500, "Não foi possível obter as transações");
        }
    }
}