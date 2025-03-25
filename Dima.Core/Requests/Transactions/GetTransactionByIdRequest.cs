namespace Dima.Core.Requests.Transactions;

public class GetTransactionByIdRequest : RequestBase
{
    public long Id { get; set; }
}