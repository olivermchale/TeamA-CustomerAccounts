using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamA.CustomerAccounts.Data;
using TeamA.CustomerAccounts.Data.Models;

namespace TeamA.CustomerAccounts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        // todo: move EF calls out into EF repository
        // split into two controllers : account / account delete
        // change post to put.

        private readonly AccountsDb _context;

        public AccountsController(AccountsDb context)
        {
            _context = context;
        }

        [HttpGet("getCustomers")]
        public async Task<IActionResult> GetAccounts()
        {
            var accounts = await _context.CustomerAccounts
                                                    .Select(b => new CustomerAccount
                                                    {
                                                        ID = b.ID,
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

            return Ok(accounts);
        }

        [HttpGet("getCustomer")]
        public async Task<IActionResult> GetAccount(Guid accountId)
        {
            var account = await _context.CustomerAccounts
                                            .Select(b => new CustomerAccount
                                            {
                                                ID = b.ID,
                                                FirstName = b.FirstName,
                                                LastName = b.LastName,
                                                Email = b.Email,
                                                Address = b.Address,
                                                Postcode = b.Postcode,
                                                DOB = b.DOB,
                                                LoggedOnAt = b.LoggedOnAt,
                                                PhoneNumber = b.PhoneNumber,
                                                CanPurchase = b.CanPurchase,
                                            }).Where(a => a.ID == accountId).FirstOrDefaultAsync();

            return Ok(account);
        }

        [HttpPost("requestAccountDelete")]
        public async Task<IActionResult> RequestAccountDelete(Guid accountId)
        {
            var account = await _context.CustomerAccounts.Where(a => a.ID == accountId).FirstOrDefaultAsync();

            if (account != null)
            {
                account.IsDeleteRequested = true;

                _context.CustomerAccounts.Update(account);

                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpGet("getRequestedDeletes")]
        public async Task<IActionResult> GetRequestedDeletes()
        {
            var accounts = await _context.CustomerAccounts.Where(a => a.IsDeleteRequested == true).ToListAsync();

            if (accounts != null)
            {
                return Ok(accounts);
            }

            return NotFound();
        }


        [HttpPost("deleteAccount")]
        public async Task<IActionResult> DeleteAccount(Guid accountId)
        {
            var account = await _context.CustomerAccounts.Where(a => a.ID == accountId).FirstOrDefaultAsync();

            if (account != null)
            {
                account.IsDeleted = true;

                _context.CustomerAccounts.Update(account);

                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost("updatePurchaseAbility")]
        public async Task<IActionResult> UpdatePurchaseAbility(Guid accountId, bool canPurchase)
        {
            var account = await _context.CustomerAccounts.Where(a => a.ID == accountId).FirstOrDefaultAsync();

            if (account != null)
            {
                account.CanPurchase = canPurchase;

                _context.CustomerAccounts.Update(account);

                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}