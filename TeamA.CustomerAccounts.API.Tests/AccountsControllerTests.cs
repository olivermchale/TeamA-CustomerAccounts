using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamA.CustomerAccounts.API.Controllers;
using TeamA.CustomerAccounts.Models;
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
        private UpdatePurchaseAbilityVm _stubEnableUpdatePurchaseAbilityVm;
        private UpdatePurchaseAbilityVm _stubDisableUpdatePurchaseAbilityVm;
        private UpdateUserVm _stubUpdateUserVm;
        private CustomerAccountDto _stubCustomerAccountDto;

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

            _stubEnableUpdatePurchaseAbilityVm = new UpdatePurchaseAbilityVm()
            {
                AccountId = new Guid("58dfa3d3-83e3-490f-97f4-3290037ea364"),
                PurchaseAbility = true
            };
            _stubDisableUpdatePurchaseAbilityVm = new UpdatePurchaseAbilityVm()
            {
                AccountId = new Guid("58dfa3d3-83e3-490f-97f4-3290037ea365"),
                PurchaseAbility = true
            };
            _stubUpdateUserVm = new UpdateUserVm()
            {
                Id = new Guid("58dfa3d3-83e3-490f-97f4-3290037ea365"),
                Address = "Test Avenue",
                Email = "Test@Unit.com@",
                FirstName = "Unit",
                LastName = "Test",
                PhoneNumber = "230954822412",
                Postcode = "T3ST 101"
            };
            _stubCustomerAccountDto = new CustomerAccountDto
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

        public async Task GetAccounts_NoAccounts_NotFound()
        {
            // Arrange 
            _mockAccountsService.Setup(c => c.GetAccounts())
                .ReturnsAsync(() => null);

            // Act
            var result = await _accountsController.GetAccounts() as NotFoundResult;

            // Assert
            Assert.AreEqual(404, result.StatusCode);

        }

        [Test]
        public async Task GetAccount_Valid()
        {
            // Arrange
            _mockAccountsService.Setup(c => c.GetAccount(It.IsAny<Guid>()))
                .ReturnsAsync(_stubCustomerAccountDetail);

            // Act
            var result = await _accountsController.GetAccount(Guid.Parse("58dfa3d3-83e3-490f-97f4-3290037ea364")) as OkObjectResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 200);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<CustomerAccountDetailVm>(result.Value);
            Assert.AreEqual(result.Value, _stubCustomerAccountDetail);
        }

        [Test]
        public void GetAccount_NoID_Throws()
        {
            // Arrange is done by setup, no mocking required as exception should get thrown.

            // Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _accountsController.GetAccount(Guid.Empty));
        }

        [Test]
        public async Task RequestAccountDelete_Valid()
        {
            // Arrange
            _mockAccountsService.Setup(c => c.RequestAccountDelete(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            // Act
            var result = await _accountsController.RequestAccountDelete(Guid.Parse("58dfa3d3-83e3-490f-97f4-3290037ea364")) as OkObjectResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 200);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<bool>(result.Value);
            Assert.AreEqual(result.Value, true);
        }

        [Test]
        public async Task RequestAccountDelete_NoID_Throws()
        {
            // Arrange is done by setup, no mocking required as exception should get thrown.

            // Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _accountsController.RequestAccountDelete(Guid.Empty));
        }

        [Test]
        public async Task RequestAccountDelete_ValidId_Failed()
        {
            // Arrange
            _mockAccountsService.Setup(c => c.RequestAccountDelete(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act
            var result = await _accountsController.RequestAccountDelete(Guid.Parse("58dfa3d3-83e3-490f-97f4-3290037ea364")) as StatusCodeResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        [Test]
        public async Task GetDeletedAccounts_Valid()
        {
            // Arrange
            _mockAccountsService.Setup(c => c.GetRequestedDeletes())
                .ReturnsAsync(_stubAccountList);

            // Act
            var result = await _accountsController.GetRequestedDeletes() as OkObjectResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 200);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<CustomerAccountListVm>(result.Value);
            Assert.AreEqual(result.Value, _stubAccountList);
        }

        [Test]
        public async Task GetDeletedAccounts_NoAccounts_NotFound()
        {
            // Arrange
            _mockAccountsService.Setup(c => c.GetRequestedDeletes())
                .ReturnsAsync(() => null);

            // Act
            var result = await _accountsController.GetRequestedDeletes() as NotFoundResult;

            // Assert
            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public async Task DeleteAccount_Valid()
        {
            // Arrange
            _mockAccountsService.Setup(c => c.DeleteAccount(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            // Act
            var result = await _accountsController.DeleteAccount(Guid.Parse("58dfa3d3-83e3-490f-97f4-3290037ea364")) as OkObjectResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 200);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<bool>(result.Value);
            Assert.AreEqual(result.Value, true);
        }

        [Test]
        public async Task DeleteAccount_Fails_Gives500()
        {
            // Arrange
            _mockAccountsService.Setup(c => c.DeleteAccount(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act
            var result = await _accountsController.DeleteAccount(Guid.Parse("58dfa3d3-83e3-490f-97f4-3290037ea364")) as StatusCodeResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        [Test]
        public async Task UpdatePurchaseAbility_EnableAbility_Valid()
        {
            // Arrange
            _mockAccountsService.Setup(c => c.UpdatePurchaseAbility(It.IsAny<UpdatePurchaseAbilityVm>()))
                .ReturnsAsync(true);

            // Act
            var result = await _accountsController.UpdatePurchaseAbility(_stubEnableUpdatePurchaseAbilityVm) as OkObjectResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 200);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<bool>(result.Value);
            Assert.AreEqual(result.Value, true);
        }

        [Test]
        public async Task UpdatePurchaseAbility_DisableAbility_Valid()
        {
            // Arrange
            _mockAccountsService.Setup(c => c.UpdatePurchaseAbility(It.IsAny<UpdatePurchaseAbilityVm>()))
                .ReturnsAsync(true);

            // Act
            var result = await _accountsController.UpdatePurchaseAbility(_stubDisableUpdatePurchaseAbilityVm) as OkObjectResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 200);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<bool>(result.Value);
            Assert.AreEqual(result.Value, true);
        }

        [Test]
        public async Task UpdatePurchaseAbility_Error_Gives500()
        {
            // Arrange
            _mockAccountsService.Setup(c => c.UpdatePurchaseAbility(It.IsAny<UpdatePurchaseAbilityVm>()))
                .ReturnsAsync(false);

            // Act
            var result = await _accountsController.UpdatePurchaseAbility(_stubDisableUpdatePurchaseAbilityVm) as StatusCodeResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        [Test]
        public async Task UpdateUser_Valid()
        {
            // Arrange
            _mockAccountsService.Setup(c => c.UpdateUser(It.IsAny<UpdateUserVm>()))
                .ReturnsAsync(true);

            // Act
            var result = await _accountsController.UpdateUser(_stubUpdateUserVm) as OkObjectResult;

            // Assert
            Assert.AreEqual(result.StatusCode, 200);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<bool>(result.Value);
            Assert.AreEqual(result.Value, true);
        }

        [Test]
        public async Task UpdateUser_Error_Gives500()
        {
            // Arrange
            _mockAccountsService.Setup(c => c.UpdateUser(It.IsAny<UpdateUserVm>()))
                .ReturnsAsync(false);

            // Act
            var result = await _accountsController.UpdateUser(_stubUpdateUserVm) as StatusCodeResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        [Test]
        public async Task CreateCustomer_Valid_Success()
        {
            // Arrange
            _mockAccountsService.Setup(c => c.CreateAccount(It.IsAny<CustomerAccountDto>()))
                .ReturnsAsync(true);

            // Act
            var result = await _accountsController.CreateAccount(_stubCustomerAccountDto) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Value);
            Assert.AreEqual(200, result.StatusCode);
            _mockAccountsService.Verify(m => m.CreateAccount(It.IsAny<CustomerAccountDto>()), Times.Once);
        }

        [Test]
        public async Task CreateCustomer_Null_Fails()
        {
            // Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _accountsController.CreateAccount(null));
            _mockAccountsService.Verify(m => m.CreateAccount(It.IsAny<CustomerAccountDto>()), Times.Never);
        }

        [Test]
        public async Task CreateCustomer_DbFails_500()
        {
            // Arrange
            _mockAccountsService.Setup(c => c.CreateAccount(It.IsAny<CustomerAccountDto>()))
                .ReturnsAsync(false);

            // Act
            var result = await _accountsController.CreateAccount(_stubCustomerAccountDto) as StatusCodeResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
            _mockAccountsService.Verify(m => m.CreateAccount(It.IsAny<CustomerAccountDto>()), Times.Once);
        }


    }
}