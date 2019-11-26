using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamA.CustomerAccounts.Data;
using TeamA.CustomerAccounts.Models;
using TeamA.CustomerAccounts.Services;

namespace TeamA.CustomerAccounts.API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private readonly IAccountsService _accountsService;

        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        [HttpGet("getCustomers")]
        public async Task<IActionResult> GetAccounts()
        {
            var accounts = await _accountsService.GetAccounts();

            return Ok(accounts);
        }

        [HttpGet("getCustomer")]
        public async Task<IActionResult> GetAccount(Guid accountId)
        {
            var account = await _accountsService.GetAccount(accountId);

            return Ok(account);
        }

        [HttpPut("requestAccountDelete")]
        public async Task<IActionResult> RequestAccountDelete(Guid accountId)
        {
            var success = await _accountsService.RequestAccountDelete (accountId);
            if (success)
            {
                return Ok();
            }

            //todo: proper error code?
            return NotFound();
        }

        [HttpGet("getRequestedDeletes")]
        public async Task<IActionResult> GetRequestedDeletes()
        {
            var accounts = await _accountsService.GetRequestedDeletes();

            if (accounts != null)
            {
                return Ok(accounts);
            }

            return NotFound();
        }


        [HttpPut("deleteAccount")]
        public async Task<IActionResult> DeleteAccount(Guid accountId)
        {
            var success = await _accountsService.DeleteAccount(accountId);

            if(success)
            {
                return Ok();
            }

            //todo: proper error code?
            return NotFound();
        }

        [HttpPut("updatePurchaseAbility")]
        public async Task<IActionResult> UpdatePurchaseAbility(Guid accountId, bool canPurchase)
        {
            var success = await _accountsService.UpdatePurchaseAbility(accountId, canPurchase);
            if (success)
            {
                return Ok();
            }

            //todo: proper error code?
            return NotFound();
        }
    }
}