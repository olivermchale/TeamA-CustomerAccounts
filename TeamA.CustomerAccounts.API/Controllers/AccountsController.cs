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
            _logger.LogInformation("Getting all custome account with id: " + accountId);
            if (accountId == null || accountId == Guid.Empty)
            {
                _logger.LogError("Failed to get account due to invalid or no account id");
                throw new ArgumentNullException(nameof(accountId));
            }
            var account = await _accountsService.GetAccount(accountId);
            if(account != null)
            {
                _logger.LogInformation("Successfully got account with account id: " + accountId);
                return Ok(account);
            }
            _logger.LogError("Failed to get account with account id: " + accountId);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);

        }

        [AllowAnonymous]
        [HttpPost("createCustomerAccount")]
        public async Task<IActionResult> CreateAccount(CustomerAccountDto customerAccount)
        {
            _logger.LogInformation("Creating new customer account");
            if(customerAccount == null)
            {
                _logger.LogError("Failed to get create due to invalid or no account information");
                throw new ArgumentException(nameof(customerAccount));
            }
            var success = await _accountsService.CreateAccount(customerAccount);
            if(success)
            {
                _logger.LogInformation("Successfully created new account");
                return Ok(success);
            }
            _logger.LogError("Failed to get account with account info: " + customerAccount);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [Authorize(Policy = "Customer")]
        [HttpPut("requestAccountDelete")]
        public async Task<IActionResult> RequestAccountDelete(Guid accountId)
        {
            _logger.LogInformation("Requesting an account deletion with account id: " + accountId);
            if (accountId == null || accountId == Guid.Empty)
            {
                _logger.LogError("Failed to request account deletion due to invalid or no account Id");
                throw new ArgumentNullException(nameof(accountId));
            }
            var success = await _accountsService.RequestAccountDelete(accountId);
            if (success)
            {
                _logger.LogInformation("Successfully requested account deletion for account id: " + accountId);
                return Ok(true);
            }
            _logger.LogInformation("Failed to request account deletion for account Id: " + accountId);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);

        }

        [Authorize(Policy = "Staff")]
        [HttpGet("getRequestedDeletes")]
        public async Task<IActionResult> GetRequestedDeletes()
        {
            _logger.LogInformation("Getting all requested account deletions");
            var accounts = await _accountsService.GetRequestedDeletes();

            if (accounts != null)
            {
                _logger.LogInformation("Successfully got requested deletions");
                return Ok(accounts);
            }
            _logger.LogError("Failed to get requested account deletions");
            return NotFound();
        }

        [Authorize(Policy = "Staff")]
        [HttpPut("deleteAccount")]
        public async Task<IActionResult> DeleteAccount(Guid accountId)
        {
            _logger.LogInformation("Deleting account with id: " + accountId);
            var success = await _accountsService.DeleteAccount(accountId);

            if(success)
            {
                _logger.LogInformation("Successfully deleted account with id: " + accountId);
                return Ok(true);
            }
            _logger.LogError("Failed to delete account with id: " + accountId);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [Authorize(Policy = "Staff")]
        [HttpPut("updatePurchaseAbility")]
        public async Task<IActionResult> UpdatePurchaseAbility(UpdatePurchaseAbilityVm updatedPurchaseAbility)
        {
            _logger.LogInformation("Updating purchase ability of account with id: " + updatedPurchaseAbility.AccountId);
            if (updatedPurchaseAbility == null)
            {
                _logger.LogError("Failed to update purchase ability due to ivnalid of no purchase ability vm sent");
                throw new ArgumentException(nameof(updatedPurchaseAbility));
            }
            var success = await _accountsService.UpdatePurchaseAbility(updatedPurchaseAbility);
            if (success)
            {
                _logger.LogInformation("Successfully updated purchase ability for account with id: " + updatedPurchaseAbility.AccountId);
                return Ok(true);
            }
            _logger.LogError("Failed to update purchase ability for account with id: " + updatedPurchaseAbility.AccountId);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [Authorize(Policy = "Customer")]
        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserVm updatedUser)
        {
            _logger.LogInformation("Updating user with id: " + updatedUser.Id);
            if (updatedUser == null)
            {
                _logger.LogError("Failed to update user due to invalid or no user information: " + updatedUser);
            }
            var success = await _accountsService.UpdateUser(updatedUser);
            if(success)
            {
                _logger.LogInformation("Successfully updated user with id: " + updatedUser.Id);
                return Ok(true);
            }
            _logger.LogError("Failed to update user with information: " + updatedUser);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}