using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamA.CustomerAccounts.Models;
using TeamA.CustomerAccounts.Models.ViewModels;

namespace TeamA.CustomerAccounts.Services
{
    public interface IAccountsService
    {
        Task<List<CustomerAccount>> GetAccounts();
        Task<CustomerAccount> GetAccount(Guid accountId);
        Task<bool> RequestAccountDelete(Guid accountId);
        Task<List<CustomerAccount>> GetRequestedDeletes();
        Task<bool> DeleteAccount(Guid accountId);
        Task<bool> UpdatePurchaseAbility(Guid accountId, bool canPurchase);
        Task<bool> UpdateUser(UpdateUserVm updatedUser);
    }
}
