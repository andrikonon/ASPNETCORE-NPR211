
using System.ComponentModel.DataAnnotations;

namespace WebPizzaSite.Models.Category;

public class CategoryCreateViewModel
{
    [Display(Name = "Назва категорії")]
    [Required(ErrorMessage = "Вкажіть назву категорії")]
    public string Name { get; set; } = string.Empty;
    [Display(Name = "Опис категорії")]
    [Required(ErrorMessage = "Вкажіть опис категорії")]
    public string Description { get; set; } = string.Empty;
    [Display(Name = "Оберіть фото")]
    [Required(ErrorMessage = "Оберіть фото категорії на ПК")]
    [DataType(DataType.Upload)]
    public IFormFile? Image { get; set; }
}