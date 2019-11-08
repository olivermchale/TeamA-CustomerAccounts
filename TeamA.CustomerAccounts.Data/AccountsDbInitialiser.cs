using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamA.CustomerAccounts.Data.Models;

namespace TeamA.CustomerAccounts.Data
{
    public class AccountsDbInitialiser
    {
        public static async Task SeedTestData(AccountsDb context, IServiceProvider services)
        {
            if (context.CustomerAccounts.Any())
            {
                // db is seeded
                return;
            }

            var accounts = new List<CustomerAccount>
            {
                new CustomerAccount
                {
                    FirstName = "Oliver",
                    LastName = "McHale",
                    Email = "OliverMchale@TeamA.com",
                    Address = "Teesside University",
                    Postcode = "TS1 OLI",
                    DOB = new DateTime(1998, 6, 24),
                    PhoneNumber = "0772092490201"
                },
                new CustomerAccount
                {
                    FirstName = "Stevie",
                    LastName = "Cartmail",
                    Email = "StevieC@TeamA.com",
                    Address = "Teesside University",
                    Postcode = "TS1 STE",
                    DOB = new DateTime(1998, 6, 24),
                    PhoneNumber = "0772092490201"
                },
                new CustomerAccount
                {
                    FirstName = "Kyle",
                    LastName = "Spence",
                    Email = "KyleS@TeamA.com",
                    Address = "Teesside University",
                    Postcode = "TS1 STE",
                    DOB = new DateTime(1998, 6, 24),
                    PhoneNumber = "0772092490201"
                },
                new CustomerAccount
                {
                    FirstName = "Oliver",
                    LastName = "McBurney",
                    Email = "OliMcB@TeamA.com",
                    Address = "Teesside University",
                    Postcode = "TS1 STE",
                    DOB = new DateTime(1998, 6, 24),
                    PhoneNumber = "0772092490201"
                },
                new CustomerAccount
                {
                    FirstName = "Craig",
                    LastName = "Martin",
                    Email = "CraigyM@TeamA.com",
                    Address = "Teesside University",
                    Postcode = "TS1 STE",
                    DOB = new DateTime(1998, 6, 24),
                    PhoneNumber = "0772092490201"
                },
            };
            accounts.ForEach(a => context.CustomerAccounts.Add(a));

            await context.SaveChangesAsync();
        }
    }
}
