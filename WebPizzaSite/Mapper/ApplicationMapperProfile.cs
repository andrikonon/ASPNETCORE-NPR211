using AutoMapper;
using WebPizzaSite.Data.Entities;
using WebPizzaSite.Models.Category;
using WebPizzaSite.Models.Product;

namespace WebPizzaSite.Mapper;

public class ApplicationMapperProfile : Profile
{
    public ApplicationMapperProfile()
    {
        CreateMap<CategoryEntity, CategoryItemViewModel>();

        CreateMap<CategoryCreateViewModel, CategoryEntity>()
            .ForMember(x=>x.Image, opt=>opt.Ignore());

        CreateMap<ProductEntity, ProductItemViewModel>()
            .ForMember(x => x.CategoryName, opt => 
                opt.MapFrom(x => x.Category == null ? String.Empty : x.Category.Name))
            .ForMember(x => x.Images, opt =>
                opt.MapFrom(x => x.ProductImages == null ? 
                    new List<string>() : x.ProductImages.Select(pi=>pi.Name).ToList()));

        CreateMap<ProductCreateViewModel, ProductEntity>()
            .ForMember(x => x.ProductImages, opt => opt.Ignore());
    }
}