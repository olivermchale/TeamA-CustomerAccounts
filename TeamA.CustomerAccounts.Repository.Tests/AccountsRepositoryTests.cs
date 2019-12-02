using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamA.CustomerAccounts.Data;
using TeamA.CustomerAccounts.Models;
using TeamA.CustomerAccounts.Models.ViewModels;
using TeamA.CustomerAccounts.Repository;
using TeamA.CustomerAccounts.Services;

namespace Tests
{
    public class AccountsRepositoryTests
    {
        private IAccountsService _accountsService;
        private List<CustomerAccountDto> _stubAccounts;

        [SetUp]
        public void Setup()
        {
            _stubAccounts = new List<CustomerAccountDto>
            {
                new CustomerAccountDto
                {
                    Id = Guid.Parse("bdca4f88-5868-4998-b5b1-6d7b77918c9b"),
                    FirstName = "Oliver",
                    LastName = "McHale",
                    Email = "OliverMchale@TeamA.com",
                    Address = "Teesside University",
                    Postcode = "TS1 OLI",
                    DOB = new DateTime(1998, 6, 24),
                    PhoneNumber = "0772092490201"
                },
                new CustomerAccountDto
                {
                    Id = Guid.Parse("90b68827-300d-4e46-8004-db064ccd98d5"),
                    FirstName = "Stevie",
                    LastName = "Cartmail",
                    Email = "StevieC@TeamA.com",
                    Address = "Teesside University",
                    Postcode = "TS1 STE",
                    DOB = new DateTime(1998, 6, 24),
                    PhoneNumber = "0772092490201"
                },
                new CustomerAccountDto
                {
                    Id = Guid.Parse("760ee42b-bd47-46eb-8fd3-d5d02760dd6c"),
                    FirstName = "Kyle",
                    LastName = "Spence",
                    Email = "KyleS@TeamA.com",
                    Address = "Teesside University",
                    Postcode = "TS1 STE",
                    DOB = new DateTime(1998, 6, 24),
                    PhoneNumber = "0772092490201"
                },
                new CustomerAccountDto
                {
                    Id = Guid.Parse("ea8cccdc-d751-4adb-a0e1-0e0c4d0b75f5"),
                    FirstName = "Oliver",
                    LastName = "McBurney",
                    Email = "OliMcB@TeamA.com",
                    Address = "Teesside University",
                    Postcode = "TS1 STE",
                    DOB = new DateTime(1998, 6, 24),
                    PhoneNumber = "0772092490201",
                    IsDeleteRequested = true
                },
                new CustomerAccountDto
                {
                    Id = Guid.Parse("bf5a195f-872e-4669-8f36-1d6d9d6dad40"),
                    FirstName = "Craig",
                    LastName = "Martin",
                    Email = "CraigyM@TeamA.com",
                    Address = "Teesside University",
                    Postcode = "TS1 STE",
                    DOB = new DateTime(1998, 6, 24),
                    PhoneNumber = "0772092490201"
                },
            };
            _accountsService = new AccountsRepository(GetMockContextWithSeedData());
        }

        private AccountsDb GetMockContextWithSeedData()
        {
            var options = new DbContextOptionsBuilder<AccountsDb>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                .Options;
            var context = new AccountsDb(options);
            _stubAccounts.ForEach(a => context.CustomerAccounts.Add(a));

            context.SaveChanges();

            return context;
        }
        

        [Test]
        public async Task GetAccounts_Valid()
        {
            // Act
            var accounts = await _accountsService.GetAccounts();

            // Assert
            Assert.IsNotNull(accounts);
            Assert.IsInstanceOf<CustomerAccountListVm>(accounts);
            Assert.AreEqual(5, accounts.CustomerAccounts.Count);
            var i = 0;
            foreach(CustomerAccountListItemVm account in accounts.CustomerAccounts)
            {
                Assert.AreEqual(account.Id, _stubAccounts[i].Id);
                Assert.AreEqual(account.FirstName, _stubAccounts[i].FirstName);
                Assert.AreEqual(account.LastName, _stubAccounts[i].LastName);
                Assert.AreEqual(account.Email, _stubAccounts[i].Email);
                Assert.AreEqual(account.IsDeleteRequested, _stubAccounts[i].IsDeleteRequested);
                i++;
            }
        }

        [Test]
        public async Task GetAccount_Valid()
        {
            // Act
            var account = await _accountsService.GetAccount(Guid.Parse("bdca4f88-5868-4998-b5b1-6d7b77918c9b"));

            // Assert
            Assert.IsNotNull(account);
            Assert.IsInstanceOf<CustomerAccountDetailVm>(account);
            Assert.AreEqual(account.Address, _stubAccounts[0].Address);
            Assert.AreEqual(account.CanPurchase, _stubAccounts[0].CanPurchase);
            Assert.AreEqual(account.DOB, _stubAccounts[0].DOB);
            Assert.AreEqual(account.Email, _stubAccounts[0].Email);
            Assert.AreEqual(account.FirstName, _stubAccounts[0].FirstName);
            Assert.AreEqual(account.LastName, _stubAccounts[0].LastName);
            Assert.AreEqual(account.Id, _stubAccounts[0].Id);
            Assert.AreEqual(account.IsActive, _stubAccounts[0].IsActive);
            Assert.AreEqual(account.IsDeleteRequested, _stubAccounts[0].IsDeleteRequested);
            Assert.AreEqual(account.LoggedOnAt, _stubAccounts[0].LoggedOnAt);
            Assert.AreEqual(account.PhoneNumber, _stubAccounts[0].PhoneNumber);
            Assert.AreEqual(account.Postcode, _stubAccounts[0].Postcode);
        }

        [Test]
        public async Task RequestAccountDelete_Valid()
        {
            // Act
            var account = await _accountsService.GetAccount(Guid.Parse("90b68827-300d-4e46-8004-db064ccd98d5"));
            Assert.IsFalse(account.IsDeleteRequested);
            var success = await _accountsService.RequestAccountDelete(Guid.Parse("90b68827-300d-4e46-8004-db064ccd98d5"));

            // Assert
            Assert.IsTrue(success);

            // Now we need to check that it actually is deleted from the mock
            var updatedAccount = await _accountsService.GetAccount(Guid.Parse("90b68827-300d-4e46-8004-db064ccd98d5"));
            Assert.IsTrue(updatedAccount.IsDeleteRequested);
        }

        [Test]
        public async Task GetRequestedDeletes_Valid()
        {
            // Act
            var requestedDeletes = await _accountsService.GetRequestedDeletes();

            // Assert
            Assert.IsNotNull(requestedDeletes);
            Assert.AreEqual(1, requestedDeletes.CustomerAccounts.Count);
            var account = requestedDeletes.CustomerAccounts[0];
            Assert.AreEqual(account.Id, _stubAccounts[3].Id);
            Assert.AreEqual(account.FirstName, _stubAccounts[3].FirstName);
            Assert.AreEqual(account.LastName, _stubAccounts[3].LastName);
            Assert.AreEqual(account.Email, _stubAccounts[3].Email);
            Assert.AreEqual(account.IsDeleteRequested, _stubAccounts[3].IsDeleteRequested);
         }

        [Test]
        public async Task GetRequestedDeletes_RequestDelete_Valid()
        {
            // Act
            var requestedDeletes = await _accountsService.GetRequestedDeletes();

            // Assert
            Assert.IsNotNull(requestedDeletes);
            Assert.AreEqual(1, requestedDeletes.CustomerAccounts.Count);

            // Act
            var success = await _accountsService.RequestAccountDelete(Guid.Parse("90b68827-300d-4e46-8004-db064ccd98d5"));
            Assert.IsTrue(success);

            var updatedRequestedDeletes = await _accountsService.GetRequestedDeletes();
            Assert.IsNotNull(updatedRequestedDeletes);
            Assert.AreEqual(2, updatedRequestedDeletes.CustomerAccounts.Count);
        }

        [Test]
        public async Task DeleteAccount_Valid()
        {
            var account = await _accountsService.GetAccount(Guid.Parse("760ee42b-bd47-46eb-8fd3-d5d02760dd6c"));
            Assert.IsTrue(account.IsActive);
            // Act
            var success = await _accountsService.DeleteAccount(Guid.Parse("760ee42b-bd47-46eb-8fd3-d5d02760dd6c"));

            var deletedAccount = await _accountsService.GetAccount(Guid.Parse("760ee42b-bd47-46eb-8fd3-d5d02760dd6c"));
            // Assert
            Assert.IsFalse(deletedAccount.IsActive);
        }

        [Test]
        public async Task UpdatePurchaseAbility_Valid()
        {
            var account = await _accountsService.GetAccount(Guid.Parse("760ee42b-bd47-46eb-8fd3-d5d02760dd6c"));
            Assert.IsTrue(account.CanPurchase);

            var success = await _accountsService.UpdatePurchaseAbility(new UpdatePurchaseAbilityVm
            {
                AccountId = Guid.Parse("760ee42b-bd47-46eb-8fd3-d5d02760dd6c"),
                PurchaseAbility = false
            });
            Assert.IsTrue(success);

            var updatedAccount = await _accountsService.GetAccount(Guid.Parse("760ee42b-bd47-46eb-8fd3-d5d02760dd6c"));
            Assert.IsFalse(updatedAccount.CanPurchase);
        }

        [Test]
        public async Task UpdateUser_Valid()
        {
            var account = await _accountsService.GetAccount(Guid.Parse("bf5a195f-872e-4669-8f36-1d6d9d6dad40"));
            Assert.AreEqual(account.FirstName, _stubAccounts[4].FirstName);
            Assert.AreEqual(account.LastName, _stubAccounts[4].LastName);
            Assert.AreEqual(account.Postcode, _stubAccounts[4].Postcode);
            Assert.AreEqual(account.Email, _stubAccounts[4].Email);
            Assert.AreEqual(account.Address, _stubAccounts[4].Address);
            Assert.AreEqual(account.PhoneNumber, _stubAccounts[4].PhoneNumber);

            var success = await _accountsService.UpdateUser(new UpdateUserVm
            {
                Id = Guid.Parse("bf5a195f-872e-4669-8f36-1d6d9d6dad40"),
                Address = "UpdatedAddress",
                Email = "UpdatedEmail",
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                PhoneNumber = "UpdatedPhoneNumber",
                Postcode = "UpdatedPostcode",
            });

            Assert.IsTrue(success);

            var updatedAccount = await _accountsService.GetAccount(Guid.Parse("bf5a195f-872e-4669-8f36-1d6d9d6dad40"));
            Assert.AreEqual(updatedAccount.FirstName, "UpdatedFirstName");
            Assert.AreEqual(updatedAccount.LastName, "UpdatedLastName");
            Assert.AreEqual(updatedAccount.Postcode, "UpdatedPostcode");
            Assert.AreEqual(updatedAccount.Email, "UpdatedEmail");
            Assert.AreEqual(updatedAccount.Address, "UpdatedAddress");
            Assert.AreEqual(updatedAccount.PhoneNumber, "UpdatedPhoneNumber");

        }
    }
}