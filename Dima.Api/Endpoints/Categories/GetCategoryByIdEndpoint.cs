using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories;

public class GetCategoryByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandleAsync)
            .WithName("Categories: Get by Id")
            .WithSummary("Get a single category")
            .WithDescription("Recupera uma categoria")
            .WithOrder(5)
            .Produces<ResponseBase<Category?>>();
    private static async Task<IResult> HandleAsync(
        ICategoryHandler handler,
        long id)

    {
        var request = new GetCategoryByIdRequest()
        {
            UserId = "teste",
            Id = id
        };
        var result = await handler.GetByIdAsyncCategory(request);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.BadRequest(result);
    }
}