using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Categories
{
    public class CreateCategoryRequest : RequestBase
    {
        [Required(ErrorMessage = "Informe um valor para o campo Title")]
        [MaxLength(80, ErrorMessage = "O titulo deve ter 80 caracteres")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Informe um valor para o campo Description")]
        public string Description { get; set; } = string.Empty;
    }
}
