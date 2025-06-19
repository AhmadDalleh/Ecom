using Ecom.Core.DTOs;
using Ecom.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrdersAsync(OrderDto orderDto, string BuyerEmail);
        Task<IReadOnlyList<Order>> GetAllOrdersForUserAsync(string BuyerEmail);
        Task<Order>GetOrderByIdAsync(int Id, string BuyerEmail);
        Task<IReadOnlyList<DeliveryMethod>>GetDeliveryMethodsAsync();
        
    }
}
