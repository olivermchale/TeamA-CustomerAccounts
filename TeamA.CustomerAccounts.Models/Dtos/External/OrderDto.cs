using System;
using System.Collections.Generic;
using System.Text;

namespace TeamA.CustomerAccounts.Models.Dtos.External
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid CustomerId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime PurchasedOn { get; set; }
    }
}
