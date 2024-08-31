using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebPizzaSite.Data;
using WebPizzaSite.Data.Entities;
using WebPizzaSite.Models.Product;

namespace WebPizzaSite.Controllers;

public class ProductController : Controller
{
    private readonly PizzaDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMapper _mapper;

    public ProductController(PizzaDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        var list = _context.Products
            .ProjectTo<ProductItemViewModel>(_mapper.ConfigurationProvider)
            .ToList();
        return View(list);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var catlist = _context.Categories
            .Select(x => new { Value = x.Id, Text = x.Name })
            .ToList();
        ProductCreateViewModel model = new();
        model.CategoryList = new SelectList(catlist, "Value", "Text");
        
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var prod = new ProductEntity
        {
            Name = model.Name,
            Price = model.Price,
            CategoryId = model.CategoryId,
        };
        
        await _context.Products.AddAsync(prod);
        await _context.SaveChangesAsync();
        if (model.Photos != null)
        {
            int i = 0;

            foreach (var img in model.Photos)
            {
                string ext = System.IO.Path.GetExtension(img.FileName);
                string fileName = Guid.NewGuid() + ext;
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await img.CopyToAsync(stream);
                }

                var imgEntity = new ProductImageEntity
                {
                    Name = fileName,
                    Priority = i++,
                    Product = prod,
                };
                _context.ProductImages.Add(imgEntity);
                _context.SaveChanges();
            }
        }

        return RedirectToAction("Index");
    }
}