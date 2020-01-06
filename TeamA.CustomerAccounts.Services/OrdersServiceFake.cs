using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamA.CustomerAccounts.Models.Dtos.External;

namespace TeamA.CustomerAccounts.Services
{
    public class OrdersServiceFake : IOrdersService
    {
        public async Task<List<OrderDto>> GetOrdersByCustomer(Guid customerId, string token)
        {
            var fakeOrder = new OrderDto
            {
                CustomerId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                InvoiceId = Guid.NewGuid(),
                OrderStatus = OrderStatus.Delivered,
                PurchasedOn = new DateTime()
            };
            List<OrderDto> fakeOrders = new List<OrderDto>() { fakeOrder };
            return await Task.FromResult(fakeOrders);
        }
    }
}
