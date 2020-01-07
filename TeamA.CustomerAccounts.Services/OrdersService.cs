using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TeamA.CustomerAccounts.Models.Dtos.External;

namespace TeamA.CustomerAccounts.Services
{
    public class OrdersService : IOrdersService
    {
        // Demonstrate Deployment
        private readonly HttpClient _client;
        private readonly ILogger<OrdersService> _logger;
        public OrdersService(HttpClient client, ILogger<OrdersService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<List<OrderDto>> GetOrdersByCustomer(Guid customerId, string token)
        {
            try
            {
                _client.DefaultRequestHeaders.Add("Authorization", token);
                _logger.LogInformation("Getting orders by customer id:" + customerId);
                using (HttpResponseMessage response = await _client.GetAsync("api/orders/customer/" + customerId))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Successfully got orders for customer: " + customerId);
                        var orders = await response.Content.ReadAsAsync<List<OrderDto>>();
                        return orders;
                    }
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Exception when getting order for customer id: " + e + e.StackTrace);
                return null;
            }
            return null;
        }
    }
}
