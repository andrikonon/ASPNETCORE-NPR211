using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPizzaSite.Data;
using WebPizzaSite.Data.Entities;
using WebPizzaSite.Models.Category;

namespace WebPizzaSite.Controllers;

public class MainController : Controller
{
    private readonly PizzaDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMapper _mapper;

    public MainController(PizzaDbContext context,
        IMapper mapper,
        IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        var list = _context.Categories
            .ProjectTo<CategoryItemViewModel>(_mapper.ConfigurationProvider)
            .ToList();
        
        return View(list);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            var cat = _mapper.Map<CategoryEntity>(model);

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var extension = Path.GetExtension(model.ImageFile.FileName);
                string filename = $"{Guid.NewGuid()}{extension}";
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", filename);
                var dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir) && dir != null)
                {
                    Directory.CreateDirectory(dir);
                }
                using (var stream = new FileStream(path, FileMode.Create))
                { 
                    await model.ImageFile.CopyToAsync(stream);
                }
                cat.Image = filename;
            }
            
            _context.Categories.Add(cat);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }
    
    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var cat = _context.Categories.SingleOrDefault(m => m.Id == id);
        if (cat == null)
        {
            return NotFound();
        }
        var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", cat.Image);
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
        _context.Categories.Remove(cat);
        _context.SaveChanges();

        return Ok(cat);
    }
}