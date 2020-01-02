using System;
using System.Collections.Generic;
using System.Text;

namespace TeamA.CustomerAccounts.Models.ViewModels
{
    public class CustomerAccountListItemVm
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public bool IsDeleteRequested { get; set; }

        public bool IsActive { get; set; }
    }
}
