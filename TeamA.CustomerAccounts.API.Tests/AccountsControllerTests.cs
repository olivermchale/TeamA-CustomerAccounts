using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamA.CustomerAccounts.API.Controllers;
using TeamA.CustomerAccounts.Models.ViewModels;
using TeamA.CustomerAccounts.Services;

namespace Tests
{
    public class AccountsControllerTest
    {
        private AccountsController _accountsController;
        private Mock<IAccountsService> _mockAccountsService;
        private CustomerAccountDetailVm _stubCustomerAccountDetail;
        private CustomerAccountListItemVm _stubCustomerAccountListItem;
        private CustomerAccountListVm _stubAccountList;



        [SetUp]
        public void Setup()
        {
            _mockAccountsService = new Mock<IAccountsService>();
            _accountsController = new AccountsController(_mockAccountsService.Object);
            _stubCustomerAccountDetail = new CustomerAccountDetailVm
            {
                Id = new Guid("58dfa3d3-83e3-490f-97f4-3290037ea365"),
                FirstName = "Oliver",
                LastName = "McHale",
                Address = "Test Drive",
                Postcode = "TS23 TST",
                DOB = new DateTime(),
                IsActive = true,
                CanPurchase = true,
                LoggedOnAt = new DateTime(),
                Email = "Oliver@Unit.Test.com",
                PhoneNumber = "01642652413",
                IsDeleteRequested = false
            };

            _stubCustomerAccountListItem = new CustomerAccountListItemVm
            {
                FirstName = "Oliver",
                LastName = "McHale",
                Address = "Test Close",
                Email = "Oliver.Does.Testing@UnitTest.com",
                IsDeleteRequested = false,
                Id = new Guid("58dfa3d3-83e3-490f-97f4-3290037ea364")
            };

            var stubListItems = new List<CustomerAccountListItemVm>();
            stubListItems.Add(_stubCustomerAccountListItem);

            _stubAccountList = new CustomerAccountListVm
            {
                CustomerAccounts = stubListItems
            };
        }

        [Test]
        public async Task GetAccounts_Valid()
        {
            // Arrange 
            _mockAccountsService.Setup(c => c.GetAccounts())
                .ReturnsAsync(_stubAccountList);

            // Act

            // Take as OkObjectResult as if it isn't, the test will fail. We assert that it is
            // Indeed Ok by asserting on the status code.
            var result = await _accountsController.GetAccounts() as OkObjectResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 200);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<CustomerAccountListVm>(result.Value);
            Assert.AreEqual(result.Value, _stubAccountList);
        }

        [Test]
        public async Task GetAccount_Valid()
        {
            // Arrange
            _mockAccountsService.Setup(c => c.GetAccount(It.IsAny<Guid>()))
                .ReturnsAsync(_stubCustomerAccountDetail);

            // Act
            var result = await _accountsController.GetAccount(new Guid()) as OkObjectResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 200);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<CustomerAccountDetailVm>(result.Value);
            Assert.AreEqual(result.Value, _stubCustomerAccountDetail);
        }
    }
}