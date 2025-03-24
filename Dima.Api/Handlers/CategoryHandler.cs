using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers
{
    public class CategoryHandler : ICategoryHandler
    {
        private readonly AppDbContext _context;
        public CategoryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseBase<Category?>> CreatAsyncCategory(CreateCategoryRequest request)
        {
            try
            {
                var category = new Category
                {
                    UserId = request.UserId,
                    Title = request.Title,
                    Description = request.Description
                };

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return new ResponseBase<Category?>(category, 201, "Categoria criada com sucesso");
            }
            catch
            {
                return new ResponseBase<Category?>(null, 500, "Categoria nao criada");
            }
        }

        public async Task<ResponseBase<Category?>> DeleteAsyncCategory(DeleteCategoryRequest request)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
                if (category == null)
                {
                    return new ResponseBase<Category?>(null, 404, "Categoria nao encontrada");
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return new ResponseBase<Category?>(category, message: "Categoria removida com sucesso");

            }
            catch
            {
                return new ResponseBase<Category?>(null, 500, "Nao foi possivel remover a categoria");
            }
        }

        public async Task<PagedResponseBase<List<Category>>> GetAllAsyncCategory(GetAllCategoriesRequest request)
        {
            try
            {
                var categories = await _context
                    .Categories
                    .AsNoTracking()
                    .Where(x => x.UserId == request.UserId)
                    .Skip(request.PageSize * (request.PageNumber-1))
                    .Take(request.PageSize)
                    .ToListAsync();
                
                var count = await _context.Categories
                    .AsNoTracking()
                    .Where(x => x.UserId == request.UserId).CountAsync();

                return new PagedResponseBase<List<Category>>(categories, count, request.PageNumber, request.PageSize);

            }
            catch
            {
                return new PagedResponseBase<List<Category>>(null, 404, "Nenhuma categoria encontrada");
            }
        }

        public async Task<ResponseBase<Category?>> GetByIdAsyncCategory(GetCategoryByIdRequest request)
        {
            try
            {
                var category = await _context
                    .Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
                return category is null
                    ? new ResponseBase<Category?>(null, 404, "Categoria nao encontrada")
                    : new ResponseBase<Category?>(category, message: "Categoria encontrada com sucesso");

            }
            catch
            {
                return new ResponseBase<Category?>(null, 500, "Nao foi possivel recuperar a categoria");
            }
        }

        public async Task<ResponseBase<Category?>> UpdateAsyncCategory(UpdateCategoryRequest request)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
                if (category == null)
                {
                    return new ResponseBase<Category?>(null, 404, "Categoria nao encontrada");
                }

                category.Title = request.Title;
                category.Description = request.Description;

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return new ResponseBase<Category?>(category, message: "Categoria atualizada com sucesso");

            }
            catch
            {
                return new ResponseBase<Category?>(null, 500, "Nao foi possivel alterar a categoria");
            }
        }
    }
}
