using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPizzaSite.Data;
using WebPizzaSite.Data.Entities;
using WebPizzaSite.Models.Category;

namespace WebPizzaSite.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    public readonly PizzaDbContext _context;
    public readonly IConfiguration _configuration;
    public readonly IMapper _mapper;
    public CategoriesController(PizzaDbContext context, IConfiguration configuration, IMapper mapper)
    {
        _context = context;
        _configuration = configuration;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var list = await _context.Categories
            .ProjectTo<CategoryItemViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return Ok(list);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromForm]CategoryCreateViewModel model)
    {
        string imageName = string.Empty;
        if (model.ImageFile != null)
        {
            imageName = Guid.NewGuid().ToString()+".jpg";
            var dirImage =_configuration["ImageFolder"] ?? "uploading";
            var fileSave = Path.Combine(Directory.GetCurrentDirectory(), dirImage, imageName);
            using(var stream = new FileStream(fileSave, FileMode.Create))
                await model.ImageFile.CopyToAsync(stream);
        }
        var entity = _mapper.Map<CategoryEntity>(model);
        entity.Name = imageName;
        _context.Categories.Add(entity);
        _context.SaveChanges();
        return Ok(entity);
    }
}