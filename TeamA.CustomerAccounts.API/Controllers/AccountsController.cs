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
        private readonly IOrdersService _ordersService;
        private readonly ILogger<AccountsController> _logger;
        public AccountsController(IAccountsService accountsService, IOrdersService ordersService, ILogger<AccountsController> logger)
        {
            _accountsService = accountsService;
            _ordersService = ordersService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all customer accounts from
        /// </summary>
        /// <returns>A list of customer account</returns>
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

        /// <summary>
        /// Gets a customer account matching the passed ID.
        /// </summary>
        /// <param name="accountId">The customer account ID</param>
        /// <returns>A customer account with the passed ID</returns>
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

        /// <summary>
        /// Creates a new customer account
        /// </summary>
        /// <param name="customerAccount">The customer account information</param>
        /// <returns>A 200 OK object with a true value indicating success, or 500 indicating failure.</returns>
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

        /// <summary>
        /// Requests a deletion for an account given an Id.
        /// </summary>
        /// <param name="accountId">The account Id</param>
        /// <returns>A 200 OK object with a true value indicating success, or a 500 indicating failure</returns>
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

        /// <summary>
        /// Gets all requested deletions
        /// </summary>
        /// <returns>A list of account details where the user has requested a deletion of their account.</returns>
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

        /// <summary>
        /// Actual deletion for an account given an Id
        /// </summary>
        /// <param name="accountId">The account Id</param>
        /// <returns>A  200 ok object cwith a true value when successful, otherwise, a 500 error indicating failure.</returns>
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

        /// <summary>
        /// Updates the purchase ability of a given customer account
        /// </summary>
        /// <param name="updatedPurchaseAbility">The vm containing the user ID and the updated purchse ability</param>
        /// <returns>A 200 OK object with a true value indicating success, or a 500 indicating false. </returns>
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

        /// <summary>
        /// Updates a user given the current user Id and the requested update information
        /// </summary>
        /// <param name="updatedUser">An Object containing the current user Id and the requested update information</param>
        /// <returns>A 200 OK object with a true value indicating success, or a 500 indicating false. </returns>
        [Authorize(Policy = "Customer")]
        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserVm updatedUser)
        {
            _logger.LogInformation("Updating user with id: " + updatedUser.Id);
            if (updatedUser == null)
            {
                _logger.LogError("Failed to update user due to invalid or no user information: " + updatedUser);
                throw new ArgumentException(nameof(updatedUser));
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

        [Authorize(Policy = "Customer")]
        [HttpGet("getOrdersForCustomer")]
        public async Task<IActionResult> GetOrdersForCustomer(Guid customerId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            _logger.LogInformation("Getting orders for customer with id: " + customerId);
            if (customerId == null)
            {
                _logger.LogError("Failed to update user due to invalid or no user id: " + customerId);
                throw new ArgumentException(nameof(customerId));
            }
            var orders = await _ordersService.GetOrdersByCustomer(customerId, token);
            if(orders != null)
            {
                _logger.LogInformation("Successfully got orders for customer: " + customerId);
                return Ok(orders);
            }
            _logger.LogError("Failed to get orders for customer: " + customerId);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}