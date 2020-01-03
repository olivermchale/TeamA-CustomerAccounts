using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamA.CustomerAccounts.Data;
using TeamA.CustomerAccounts.Models;
using TeamA.CustomerAccounts.Models.ViewModels;
using TeamA.CustomerAccounts.Services;

namespace TeamA.CustomerAccounts.API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private readonly IAccountsService _accountsService;
        private readonly ILogger<AccountsController> _logger;
        public AccountsController(IAccountsService accountsService, ILogger<AccountsController> logger)
        {
            _accountsService = accountsService;
            _logger = logger;
        }

        [Authorize(Policy="Customer")]
        [HttpGet("getCustomers")]
        public async Task<IActionResult> GetAccounts()
        {
            _logger.LogInformation("Getting all customer accounts");
            var accounts = await _accountsService.GetAccounts();
            if(accounts != null)
            {
                return Ok(accounts);
            }
            return NotFound();

        }

        [Authorize(Policy = "Customer")]
        [HttpGet("getCustomer")]
        public async Task<IActionResult> GetAccount(Guid accountId)
        {
            if(accountId == null || accountId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(accountId));
            }
            var account = await _accountsService.GetAccount(accountId);
            if(account != null)
            {
                return Ok(account);
            }
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);

        }

        [AllowAnonymous]
        [HttpPost("createCustomerAccount")]
        public async Task<IActionResult> CreateAccount(CustomerAccountDto customerAccount)
        {
            if(customerAccount == null)
            {
                throw new ArgumentException(nameof(customerAccount));
            }
            var success = await _accountsService.CreateAccount(customerAccount);
            if(success)
            {
                return Ok(success);
            }
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [Authorize(Policy = "Customer")]
        [HttpPut("requestAccountDelete")]
        public async Task<IActionResult> RequestAccountDelete(Guid accountId)
        {
            if (accountId == null || accountId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(accountId));
            }
            var success = await _accountsService.RequestAccountDelete(accountId);
            if (success)
            {
                return Ok(true);
            }

            //todo: proper error code?
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);

        }

        [Authorize(Policy = "Staff")]
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

        [Authorize(Policy = "Staff")]
        [HttpPut("deleteAccount")]
        public async Task<IActionResult> DeleteAccount(Guid accountId)
        {
            var success = await _accountsService.DeleteAccount(accountId);

            if(success)
            {
                return Ok(true);
            }
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [Authorize(Policy = "Staff")]
        [HttpPut("updatePurchaseAbility")]
        public async Task<IActionResult> UpdatePurchaseAbility(UpdatePurchaseAbilityVm updatedPurchaseAbility)
        {
            var success = await _accountsService.UpdatePurchaseAbility(updatedPurchaseAbility);
            if (success)
            {
                return Ok(true);
            }

            //todo: proper error code?
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [Authorize(Policy = "Customer")]
        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserVm updatedUser)
        {
            var success = await _accountsService.UpdateUser(updatedUser);
            if(success)
            {
                return Ok(true);
            }
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}