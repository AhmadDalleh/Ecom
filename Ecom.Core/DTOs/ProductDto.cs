﻿using Ecom.Core.Entities.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.DTOs
{
    public record ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public virtual List<Photo> Photos { get; set; }

        public string CategoryName { get; set; }
    }

    public record ReturnProductDTO
    {
        public List<ProductDto> products { get; set; }
        public int TotalCount { get; set; }
    }
    public record PhotoDto
    {
        public string ImageName { get; set; }
        public int ProductId { get; set; }
    }


    public record AddProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public int CategoryId { get; set; }

        public IFormFileCollection Photo {  get; set; }
    }

    public record UpdateProductDTO : AddProductDTO
    {
        public int Id { get; set; }
    }
}
