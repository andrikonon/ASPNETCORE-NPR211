using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using WebPizzaSite.Data;
using WebPizzaSite.Data.Entities;
using WebPizzaSite.Models.Category;

namespace WebPizzaSite.Controllers;

public class MainController : Controller
{
    private readonly PizzaDbContext _context;
    private readonly IMapper _mapper;

    public MainController(PizzaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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
    public IActionResult Create(CategoryCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            var cat = _mapper.Map<CategoryEntity>(model);
            _context.Categories.Add(cat);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }
}