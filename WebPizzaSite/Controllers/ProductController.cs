using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebPizzaSite.Data;
using WebPizzaSite.Data.Entities;
using WebPizzaSite.Models.Product;
using IConfigurationProvider = SixLabors.ImageSharp.Advanced.IConfigurationProvider;

namespace WebPizzaSite.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : Controller
{
    public readonly PizzaDbContext _context;
    public IConfiguration _configuration;
    public readonly IMapper _mapper;

    public ProductController(PizzaDbContext context, IConfiguration configuration, IMapper mapper)
    {
        _context = context;
        _configuration = configuration;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var list = await _context.Products
            .ProjectTo<ProductItemViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return Ok(list);
    }
    //[HttpPost]
    //public async Task<IActionResult> Create([FromForm]CategoryCreateViewModel model)
    //{
    //    string imageName = string.Empty;
    //    if (model.ImageFile != null)
    //    {
    //        imageName = Guid.NewGuid().ToString()+".jpg";
    //        var dirImage =_configuration["ImageFolder"] ?? "uploading";
    //        var fileSave = Path.Combine(Directory.GetCurrentDirectory(), dirImage, imageName);
    //        using(var stream = new FileStream(fileSave, FileMode.Create))
    //            await model.ImageFile.CopyToAsync(stream);
    //    }
    //    var entity = _mapper.Map<CategoryEntity>(model);
    //    entity.Image = imageName;
    //    _context.Categories.Add(entity);
    //    _context.SaveChanges();
    //    return Ok(entity);
    //}
}