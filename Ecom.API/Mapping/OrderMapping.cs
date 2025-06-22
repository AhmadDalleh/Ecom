using AutoMapper;
using Ecom.API.Controllers;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Order;

namespace Ecom.API.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<Order,OrderToReturnDTO>().ReverseMap();
            CreateMap<OrderItem,OrderItemDTO>().ReverseMap();
            CreateMap<ShippingAddress,ShipAddressDTO>().ReverseMap();
        }
    }
}
