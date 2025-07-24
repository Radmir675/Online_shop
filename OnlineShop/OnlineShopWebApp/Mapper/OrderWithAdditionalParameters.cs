using AutoMapper;
using OnlineShopWebApp.Models;

namespace OnlineShop.db
{
    public class OrderWithAdditionalParameters : ITypeConverter<OrderViewModel, Order>
    {
        public Order Convert(OrderViewModel source, Order destination, ResolutionContext context)
        {
            destination = new Order
            {
                UserId = source.UserId,
                OrderId = source.OrderId,
                Email = source.Email,
                FullName = source.FullName,
                Number = source.Number,
                OrderStatus = (OrderStatus)source.OrderStatus,
            };
            return destination;
        }
    }
}
