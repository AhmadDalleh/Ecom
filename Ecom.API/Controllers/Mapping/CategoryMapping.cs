using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using System.Runtime;

namespace Ecom.API.Controllers.Mapping
{
    public class CategoryMapping:Profile
    {
        public CategoryMapping()
        {
            CreateMap<CategoryDTO,Category>().ReverseMap();
            CreateMap<UpdateCategoryDTO,Category>().ReverseMap();
        }
    }
}
