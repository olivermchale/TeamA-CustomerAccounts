using System;
using System.Collections.Generic;
using System.Text;

namespace TeamA.CustomerAccounts.Models.ViewModels
{
    public class UpdatePurchaseAbilityVm
    {
        public Guid AccountId { get; set; }

        public bool PurchaseAbility { get; set; }
    }
}
