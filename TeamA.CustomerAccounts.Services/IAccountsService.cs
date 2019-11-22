using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamA.CustomerAccounts.Models;

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
    }
}
