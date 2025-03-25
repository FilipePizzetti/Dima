namespace Dima.Core.Requests.Transactions;

public class GetTransactionByPeriodRequest : PagedRequestBase
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}