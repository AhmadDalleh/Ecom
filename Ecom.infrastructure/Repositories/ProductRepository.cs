using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IImageManagementService imageManagementService;
        public ProductRepository(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(ProductParams productParams)
        {
            var query = context.Products
                .Include(m=>m.Category)
                .Include(m=>m.Photos)
                .AsNoTracking();

            //filtering by word 
            if (!string.IsNullOrEmpty(productParams.Search)) 
            {
                var searchWords = productParams.Search.Split(' ');
                query = query.Where(m => searchWords.All(word =>

                m.Name.ToLower().Contains(word.ToLower())
                ||
                m.Description.ToLower().Contains(word.ToLower())

                ));
            }
                

            //filtering by category Id 
            if (productParams.CategoryId.HasValue)
               query = query.Where(m => m.CategoryId == productParams.CategoryId);
            
            //sort by price ascending and descending
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                query = productParams.Sort switch
                {
                    "PriceAcn" => query.OrderBy(m => m.NewPrice),
                    "PriceDce" => query.OrderByDescending(m => m.NewPrice),
                    _ => query.OrderBy(m => m.Name),
                };
            }

            query = query.Skip((productParams.pageSize) * (productParams.PageNumber - 1)).Take(productParams.pageSize);

            var result = mapper.Map<List<ProductDto>>(query);
            return result;
        }
        public async Task<bool> AddAsync(AddProductDTO productDto)
        {
            if (productDto == null) return false;

            var product = mapper.Map<Product>(productDto);

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            var ImagePath = await imageManagementService.AddImageAsync(productDto.Photo, productDto.Name);

            var photo = ImagePath.Select(path =>new Photo
            {
                ImageName = path,
                ProductId = product.Id,
            }).ToList();
            await context.Photos.AddRangeAsync(photo);    
            await context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync(Product product)
        {
            var photo = await context.Photos.Where(m=>m.ProductId==product.Id)
                .ToListAsync();
            foreach(var item in photo)
            {
                imageManagementService.DeleteImageAsync(item.ImageName);
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UpdateProductDTO updateProductDto)
        {
            if(updateProductDto is null)
            {
                return false;
            }
            var findProduct = await context.Products.Include(m=>m.Category)
                .Include(m => m.Photos)
                .FirstOrDefaultAsync(m=>m.Id==updateProductDto.Id);
            if(findProduct is null)
            {
                return false;
            }
            mapper.Map(updateProductDto,findProduct);

            var findPhoto = await context.Photos.Where(m=>m.ProductId == updateProductDto.Id).ToListAsync();
            foreach (var item in findPhoto) 
            {
                imageManagementService.DeleteImageAsync(item.ImageName);
            }
            context.Photos.RemoveRange(findPhoto);
            var ImagePath = await imageManagementService.AddImageAsync(updateProductDto.Photo, updateProductDto.Name);
            var photo = ImagePath.Select(path=>new Photo
            {
                ImageName = path,
                ProductId = updateProductDto.Id,
            }).ToList();
            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
