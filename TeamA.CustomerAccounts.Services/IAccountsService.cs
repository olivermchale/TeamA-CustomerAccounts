using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamA.CustomerAccounts.Models;
using TeamA.CustomerAccounts.Models.ViewModels;

namespace TeamA.CustomerAccounts.Services
{
    public interface IAccountsService
    {
        Task<CustomerAccountListVm> GetAccounts();
        Task<CustomerAccountDetailVm> GetAccount(Guid accountId);
        Task<bool> RequestAccountDelete(Guid accountId);
        Task<CustomerAccountListVm> GetRequestedDeletes();
        Task<bool> DeleteAccount(Guid accountId);
        Task<bool> UpdatePurchaseAbility(UpdatePurchaseAbilityVm updatePurchaseAbilityVm);
        Task<bool> UpdateUser(UpdateUserVm updatedUserVm);
        Task<bool> CreateAccount(CustomerAccountDto customerAccount);
    }
}
