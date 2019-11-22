using System;

namespace TeamA.CustomerAccounts.Models
{
    public class CustomerAccount
    {
        public Guid ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Postcode { get; set; }

        public DateTime DOB { get; set; }

        public DateTime LoggedOnAt { get; set; }

        public string PhoneNumber { get; set; }

        // Default to true - can be disabled manually whenever necessary
        public bool CanPurchase { get; set; } = true;

        public bool IsDeleted { get; set; } = true;

        public bool IsDeleteRequested { get; set; } = false;
    }
}
