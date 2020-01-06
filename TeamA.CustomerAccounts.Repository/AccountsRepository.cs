using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamA.CustomerAccounts.Data;
using TeamA.CustomerAccounts.Models;
using TeamA.CustomerAccounts.Models.ViewModels;
using TeamA.CustomerAccounts.Services;

namespace TeamA.CustomerAccounts.Repository
{
    public class AccountsRepository : IAccountsService
    {
        private readonly AccountsDb _context;

        private readonly ILogger<AccountsRepository> _logger;

        public AccountsRepository(AccountsDb context, ILogger<AccountsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CustomerAccountListVm> GetAccounts()
        {
            _logger.LogInformation("Getting all accounts");
            try
            {
                var accounts = await _context.CustomerAccounts
                                      .Select(b => new CustomerAccountListItemVm
                                      {
                                          Id = b.Id,
                                          FirstName = b.FirstName,
                                          LastName = b.LastName,
                                          Email = b.Email,
                                          Address = b.Address,
                                          IsDeleteRequested = b.IsDeleteRequested,
                                          IsActive = b.IsActive
                                      })
                                        .ToListAsync();

                _logger.LogInformation("Successfully got all accounts");
                return new CustomerAccountListVm
                {
                    CustomerAccounts = accounts
                };
            }
            catch (Exception e)
            {
                _logger.LogError("Exceptions when getting accounts: " + e + e.StackTrace);
                return null;
            }

        }

        public async Task<CustomerAccountDetailVm> GetAccount(Guid accountId)
        {
            _logger.LogInformation("Getting account with id: " + accountId);
            try
            {
                return await _context.CustomerAccounts
                                .Select(b => new CustomerAccountDetailVm
                                {
                                    Id = b.Id,
                                    FirstName = b.FirstName,
                                    LastName = b.LastName,
                                    Email = b.Email,
                                    Address = b.Address,
                                    Postcode = b.Postcode,
                                    DOB = b.DOB,
                                    LoggedOnAt = b.LoggedOnAt,
                                    PhoneNumber = b.PhoneNumber,
                                    CanPurchase = b.CanPurchase,
                                    IsActive = b.IsActive,
                                    IsDeleteRequested = b.IsDeleteRequested
                                }).Where(a => a.Id == accountId).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Exceptions when getting account with id : " + accountId + e + e.StackTrace);
                return null;
            }

        }

        public async Task<bool> RequestAccountDelete(Guid accountId)
        {
            _logger.LogInformation("Requesting account delete for account with id: " + accountId);
            try
            {
                var account = await _context.CustomerAccounts.Where(a => a.Id == accountId).FirstOrDefaultAsync();

                if (account != null)
                {
                    account.IsDeleteRequested = true;

                    _context.CustomerAccounts.Update(account);

                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Succesfully requested account deletion for account with id: " + accountId);
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Exceptions when requesting deletion for account with id : " + accountId + e + e.StackTrace);
            }
            return false;

        }

        public async Task<CustomerAccountListVm> GetRequestedDeletes()
        {
            _logger.LogInformation("Getting all requested deletions");
            try
            {
                var accounts = await _context.CustomerAccounts
                                      .Select(b => new CustomerAccountListItemVm
                                      {
                                          Id = b.Id,
                                          FirstName = b.FirstName,
                                          LastName = b.LastName,
                                          Email = b.Email,
                                          Address = b.Address,
                                          IsDeleteRequested = b.IsDeleteRequested,
                                      }).Where(b => b.IsDeleteRequested == true)
                                        .ToListAsync();

                return new CustomerAccountListVm
                {
                    CustomerAccounts = accounts
                };
            }
            catch (Exception e)
            {
                _logger.LogError("Exception when getting requested deletions" + e + e.StackTrace);
                return null;
            }
        }

        public async Task<bool> DeleteAccount(Guid accountId)
        {
            _logger.LogInformation("Deleting account with id: " + accountId);
            try
            {
                var account = await _context.CustomerAccounts.Where(a => a.Id == accountId).FirstOrDefaultAsync();

                if (account != null)
                {
                    account.IsActive = false;

                    _context.CustomerAccounts.Update(account);

                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Successsfully deleted account with id: " + accountId);
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception when deleting account with id: " + accountId + e + e.StackTrace);
            }
            return false;

        }

        public async Task<bool> UpdatePurchaseAbility(UpdatePurchaseAbilityVm updatedPurchaseAbility)
        {
            _logger.LogInformation("Updating purchase ability ");
            var account = await _context.CustomerAccounts.Where(a => a.Id == updatedPurchaseAbility.AccountId).FirstOrDefaultAsync();
            try
            {
                if (account != null)
                {
                    account.CanPurchase = updatedPurchaseAbility.PurchaseAbility;

                    _context.CustomerAccounts.Update(account);

                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Successfully updated purchase ability");
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception when deleting account with id: " + updatedPurchaseAbility.AccountId + e + e.StackTrace);
            }
            return false;

        }

        public async Task<bool> UpdateUser(UpdateUserVm updatedUser)
        {
            _logger.LogInformation("Updating user with id: " + updatedUser.Id);
            var account = await _context.CustomerAccounts.Where(c => c.Id == updatedUser.Id).FirstOrDefaultAsync();
            try
            {
                if (account != null)
                {
                    account.FirstName = updatedUser.FirstName;
                    account.LastName = updatedUser.LastName;
                    account.Address = updatedUser.Address;
                    account.Postcode = updatedUser.Postcode;
                    account.Email = updatedUser.Email;
                    account.PhoneNumber = updatedUser.PhoneNumber;

                    await _context.SaveChangesAsync();
                }
                _logger.LogInformation("Successfully updated user with id: " + updatedUser.Id);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception when updating user with info: " + updatedUser + e + e.StackTrace);
            }
            return false;
        }

        public async Task<bool> CreateAccount(CustomerAccountDto customerAccount)
        {
            _logger.LogInformation("Creating account with id: " + customerAccount.Id);
            try
            {
                if(customerAccount != null)
                {
                     _context.CustomerAccounts.Add(customerAccount);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Successfully created customer account");
                    return true;
                }           
            }
            catch (Exception e)
            {
                _logger.LogError("Exception when creating user with info: " + customerAccount + e + e.StackTrace);
            }
            return false;
        }
    }
}
