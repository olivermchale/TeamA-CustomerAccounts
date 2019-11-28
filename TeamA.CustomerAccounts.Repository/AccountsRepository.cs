using Microsoft.EntityFrameworkCore;
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

        public AccountsRepository(AccountsDb context)
        {
            _context = context;
        }

        public async Task<List<CustomerAccount>> GetAccounts()
        {
            return await _context.CustomerAccounts
                                                  .Select(b => new CustomerAccount
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
                                                    })
                                                    .ToListAsync();
        }

        public async Task<CustomerAccount> GetAccount(Guid accountId)
        {
            return await _context.CustomerAccounts
                                            .Select(b => new CustomerAccount
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

        public async Task<bool> RequestAccountDelete(Guid accountId)
        {
            try
            {
                var account = await _context.CustomerAccounts.Where(a => a.Id == accountId).FirstOrDefaultAsync();

                if (account != null)
                {
                    account.IsDeleteRequested = true;

                    _context.CustomerAccounts.Update(account);

                    await _context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception e)
            {
                // todo: exception handling?
            }
            return false;

        }

        public async Task<List<CustomerAccount>> GetRequestedDeletes()
        {
            return await _context.CustomerAccounts.Where(a => a.IsDeleteRequested == true).ToListAsync();
        }

        public async Task<bool> DeleteAccount(Guid accountId)
        {
            var account = await _context.CustomerAccounts.Where(a => a.Id == accountId).FirstOrDefaultAsync();
            try
            {
                if (account != null)
                {
                    account.IsActive = true;

                    _context.CustomerAccounts.Update(account);

                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception e)
            {
                //todo: exception handling
            }
            return false;

        }

        public async Task<bool> UpdatePurchaseAbility(Guid accountId, bool canPurchase)
        {
            var account = await _context.CustomerAccounts.Where(a => a.Id == accountId).FirstOrDefaultAsync();
            try
            {
                if (account != null)
                {
                    account.CanPurchase = canPurchase;

                    _context.CustomerAccounts.Update(account);

                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception e)
            {
                //todo: exception handling
            }
            return false;

        }

        public async Task<bool> UpdateUser(UpdateUserVm updatedUser)
        {
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

                return true;
            }
            catch (Exception e)
            {
                //todo: exception handling
            }
            return false;
        }

    }
}
