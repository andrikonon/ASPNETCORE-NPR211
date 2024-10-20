using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebPizzaSite.Constants;
using WebPizzaSite.Data;
using WebPizzaSite.Data.Entities;
using WebPizzaSite.Data.Entities.Identity;
using WebPizzaSite.Interfaces;
using WebPizzaSite.Mapper;
using WebPizzaSite.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IImageWorker, ImageWorker>();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PizzaDbContext>(opt => 
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IImageHulk, ImageHulk>();

builder.Services.AddAutoMapper(typeof(ApplicationMapperProfile));

builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;
        
        // options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        // options.Lockout.MaxFailedAccessAttempts = 5;
        // options.Lockout.AllowedForNewUsers = true;
        //
        // options.SignIn.RequireConfirmedEmail = true;
    })
    .AddEntityFrameworkStores<PizzaDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});


builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(opt => opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<PizzaDbContext>();
    var userManager = serviceScope.ServiceProvider.GetService<UserManager<UserEntity>>();
    var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<RoleEntity>>();
    var imageWorker = serviceScope.ServiceProvider.GetService<IImageWorker>();
    var imageHulk = serviceScope.ServiceProvider.GetService<IImageHulk>();
    
    context?.Database.Migrate();
    
    if (!context.Categories.Any())
    {
        int number = 10;
        var list = new Faker("uk")
            .Commerce.Categories(number);
        foreach (var name in list)
        {
            var image = imageHulk.Save(@"https://picsum.photos/1200/800?category").Result;
            var cat = new CategoryEntity
            {
                Name = name,
                Description = new Faker("uk").Commerce.ProductDescription(),
                Image = image,
            };
            context.Categories.Add(cat);
            context.SaveChanges();
        }
    }

    if (!context.Products.Any())
    {
        var categories = context.Categories.ToList();
        var fakerProduct = new Faker<ProductEntity>("uk")
            .RuleFor(u => u.Name, (f, u) => f.Commerce.Product())
            .RuleFor(u => u.Price, (f, u) => decimal.Parse(f.Commerce.Price()))
            .RuleFor(u => u.Category, (f, u) => f.PickRandom(categories));
        string url = "https://picsum.photos/1200/800?product";
        var products = fakerProduct.GenerateLazy(100);
        Random r = new Random();
        foreach (var product in products)
        {
            context.Add(product);
            context.SaveChanges();
            int imageCount = r.Next(3, 5);
            for (int i = 0; i < imageCount; i++)
            {
                var imageName = imageHulk.Save(url).Result;
                var imageProduct = new ProductImageEntity
                {
                    Product = product,
                    Name = imageName,
                    Priority = i
                };
                context.Add(imageProduct);
                context.SaveChanges();
            }
        }
    }

    if (!context.Products.Any())
    {
        var admin = new RoleEntity
        {
            Name = Roles.Admin,
        };
        var result = roleManager.CreateAsync(admin).Result;
        if (!result.Succeeded)
        {
            Console.WriteLine($"----- Помилка створення ролі {Roles.Admin} -----");
        }
        result = roleManager.CreateAsync(new RoleEntity{Name=Roles.User}).Result;
        if (!result.Succeeded)
        {
            Console.WriteLine($"----- Помилка створення ролі {Roles.User} -----");
        }
    }

    if (!context.Users.Any())
    {
        var user = new UserEntity
        {
            Email = "admin@gmail.com",
            UserName = "admin@gmail.com",
            LastName = "Шолом",
            FirstName = "Вулкан",
            Picture = "admin.jpg",
        };
        var result = userManager.CreateAsync(user, "123456").Result;
        if (result.Succeeded)
        {
            result = userManager.AddToRoleAsync(user, Roles.Admin).Result;
            if (!result.Succeeded)
            {
                Console.WriteLine($"----- Не вдалося надати роль {Roles.Admin} користувачу {user.Email} -----");
            }
        }
        else
        {
            Console.WriteLine($"------- Не вдалося створити користувача {user.Email} -------");
        }
    }
}

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<PizzaDbContext>();
    context?.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Main}/{action=Index}/{id?}");
#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "admin_area",
        areaName: "Admin",
        pattern: "admin/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Main}/{action=Index}/{id?}");
});
#pragma warning restore ASP0014

app.Run();