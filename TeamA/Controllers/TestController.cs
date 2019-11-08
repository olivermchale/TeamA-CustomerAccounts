using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamA.CustomerAccounts.Data;
using TeamA.CustomerAccounts.Data.Models;

namespace TeamA.Controllers
{
    [ApiController]
    public class TestController : Controller
    {

        private readonly AccountsDb _context;

        public TestController(AccountsDb context)
        {
            _context = context;
        }

        [HttpGet("api/test")]
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
    }
}