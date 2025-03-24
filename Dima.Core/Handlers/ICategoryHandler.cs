using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Core.Handlers
{
    public interface ICategoryHandler
    {
        Task<ResponseBase<Category?>> CreatAsyncCategory(CreateCategoryRequest request);
        Task<ResponseBase<Category?>> UpdateAsyncCategory(UpdateCategoryRequest request);
        Task<ResponseBase<Category?>> DeleteAsyncCategory(DeleteCategoryRequest request);
        Task<ResponseBase<Category?>> GetByIdAsyncCategory(GetCategoryByIdRequest request);
        Task<PagedResponseBase<List<Category>>> GetAllAsyncCategory(GetAllCategoriesRequest request);
    }
}
