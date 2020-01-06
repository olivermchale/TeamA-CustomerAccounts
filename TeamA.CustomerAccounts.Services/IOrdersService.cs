using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamA.CustomerAccounts.Models.Dtos.External;

namespace TeamA.CustomerAccounts.Services
{
    public interface IOrdersService
    {
        Task<List<OrderDto>> GetOrdersByCustomer(Guid customerId, string token);
    }
}
