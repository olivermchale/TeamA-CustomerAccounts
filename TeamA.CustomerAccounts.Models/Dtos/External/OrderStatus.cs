using System;
using System.Collections.Generic;
using System.Text;

namespace TeamA.CustomerAccounts.Models.Dtos.External
{
    public enum OrderStatus
    {
        Pending = 0,
        Dispatched = 1,
        Delivered = 2,
        Cancelled = 3,
        Returned = 4,
        Unknown = 5
    }
}
