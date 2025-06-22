using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Mapping
{
   public class ProductMapping : Profile
    {

        public ProductMapping()
        {
            CreateMap<Product, ProductDto>
                ().ForMember //this telling the compiler that ger Category name that exist in the DTO from the source class Category 
                (x => x.CategoryName,
                op => op.MapFrom(src => src.Category.Name))
                .ReverseMap(); 
            
            CreateMap<Photo, PhotoDto>().ReverseMap();
            CreateMap<AddProductDTO,Product>()
                .ForMember(m=>m.Photos,op=>op.Ignore())
                .ReverseMap();

            CreateMap<UpdateProductDTO, Product>()
                .ForMember(m => m.Photos, op => op.Ignore())
                .ReverseMap();
        }
    }
}
