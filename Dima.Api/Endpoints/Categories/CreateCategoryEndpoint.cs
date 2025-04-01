using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories;

public class CreateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("CreateCategory")
            .WithSummary("Creates a new category")
            .WithDescription("Cria uma nova categoria")
            .WithOrder(1)
            .Produces<ResponseBase<Category?>>();
    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ICategoryHandler handler,
        CreateCategoryRequest request)

    {
        request.UserId = user.Identity?.Name ?? string.Empty;
        var result = await handler.CreatAsyncCategory(request);
        return result.IsSuccess 
            ? TypedResults.Created($"/{result.Data?.Id}", result) 
            : TypedResults.BadRequest(result.Data);
    }
    
}