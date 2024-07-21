
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
    [Display(Name = "Фото url")]
    [Required(ErrorMessage = "Вкажіть url для фото категорії")]
    public string Image { get; set; } = string.Empty;
}