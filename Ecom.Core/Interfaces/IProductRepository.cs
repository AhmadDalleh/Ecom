using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        // for future
        Task<bool> AddAsync(AddProductDTO productDto);

        Task<bool> UpdateAsync(UpdateProductDTO updateProductDto);

        Task DeleteAsync(Product product);

    }
}
