using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebPizzaSite.Models.Product;

public class ProductCreateViewModel
{
    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Вкажіть назву продукту")]
    [StringLength(255, ErrorMessage = "Назва продукту не можу перевищувати 255 символів")]
    public string Name { get; set; }
    [Display(Name = "Ціна")]
    public decimal Price { get; set; }
    [Display(Name = "Категорія")]
    public int CategoryId { get; set; }
    [Display(Name = "Оберіть список фото продукту")]
    public List<IFormFile>? Photos { get; set; }
    public SelectList? CategoryList { get; set; }
}