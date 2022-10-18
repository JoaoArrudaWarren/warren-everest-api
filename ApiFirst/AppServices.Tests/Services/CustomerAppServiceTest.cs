﻿using AppModels.AppModels.Customer;
using AppServices.Services;
using AppServices.Tests.Fixtures.Customer;
using AutoMapper;
using DomainModels.Models;
using DomainServices.Interfaces;
using DomainServices.Tests.Fixtures;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace AppServices.Tests.Services
{
    public class CustomerAppServiceTest
    {
        private readonly CustomerAppService _customerAppService;
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICustomerBankInfoService> _customerBankInfoServiceMock;

        public CustomerAppServiceTest()
        {
            _customerServiceMock = new();
            _mapperMock = new();
            _customerBankInfoServiceMock = new();
            _customerAppService = new CustomerAppService(_customerBankInfoServiceMock.Object, _customerServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_Create_SucessFully()
        {
            var customer = CreateCustomerFixture.GenerateCustomerFixture();
            Customer custom = CustomerFixture.GenerateCustomerFixture();
            long id = 1;

            _mapperMock.Setup(p => p.Map<Customer>(It.IsAny<CreateCustomer>())).Returns(custom);
            _customerServiceMock.Setup(p => p.CreateAsync(It.IsAny<Customer>())).ReturnsAsync(id);
            _customerBankInfoServiceMock.Setup(p => p.Create(id));

            long idResult = await _customerAppService.CreateAsync(customer);

            idResult.Should().BeGreaterThanOrEqualTo(1);

            _mapperMock.Verify(p => p.Map<Customer>(It.IsAny<CreateCustomer>()), Times.Once);
            _customerServiceMock.Verify(p => p.CreateAsync(It.IsAny<Customer>()), Times.Once);
            _customerBankInfoServiceMock.Verify(p => p.Create(id), Times.Once);
        }

        [Fact]
        public async void Should_Not_Create_When_Cpf_Or_Email_Already_Exists()
        {
            var customer = CreateCustomerFixture.GenerateCustomerFixture();
            Customer custom = CustomerFixture.GenerateCustomerFixture();
            long id = 1;
            ArgumentException exc = new();

            _mapperMock.Setup(p => p.Map<Customer>(It.IsAny<CreateCustomer>())).Returns(custom);
            _customerServiceMock.Setup(p => p.CreateAsync(It.IsAny<Customer>())).Throws(exc);
            _customerBankInfoServiceMock.Setup(p => p.Create(id));

            try
            {
                long idResult = await _customerAppService.CreateAsync(customer);
                idResult.Should().BeGreaterThanOrEqualTo(1);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }

            _mapperMock.Verify(p => p.Map<Customer>(It.IsAny<CreateCustomer>()), Times.Once);
            _customerServiceMock.Verify(p => p.CreateAsync(It.IsAny<Customer>()), Times.Once);
            _customerBankInfoServiceMock.Verify(p => p.Create(id), Times.Never);
        }

        [Fact]
        public void Should_Delete_SucessFully()
        {
            long id = 1;

            _customerServiceMock.Setup(p => p.Delete(It.IsAny<long>()));

            _customerAppService.Delete(id);

            _customerServiceMock.Verify(p => p.Delete(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void Should_Not_Delete_When_Id_Doesnt_Exist()
        {
            long id = 0;
            ArgumentNullException exc = new();

            _customerServiceMock.Setup(p => p.Delete(It.IsAny<long>())).Throws(exc);

            try
            {
                _customerAppService.Delete(id);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }

            _customerServiceMock.Verify(p => p.Delete(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void Should_GetAll_SucessFully()
        {
            var customer = CustomerResponseFixture.GenerateCustomerFixture(3);
            var customers1 = new List<Customer>();

            _customerServiceMock.Setup(p => p.GetAll()).Returns(customers1);
            _mapperMock.Setup(p => p.Map<IEnumerable<CustomerResponse>>(customers1)).Returns(customer);

            var customersResponse = _customerAppService.GetAll();

            customersResponse.Should().HaveCountGreaterThanOrEqualTo(0);

            _customerServiceMock.Verify(p => p.GetAll(), Times.Once);
            _mapperMock.Verify(p => p.Map<IEnumerable<CustomerResponse>>(customers1), Times.Once);
        }

        [Fact]
        public async void Should_GetByCpf_SucessFully()
        {
            var cpf = "42713070848";
            var customer = CustomerFixture.GenerateCustomerFixture();
            var customerResponse = CustomerResponseFixture.GenerateCustomerFixture();

            _customerServiceMock.Setup(p => p.GetByCpfAsync(It.IsAny<string>())).ReturnsAsync(customer);
            _mapperMock.Setup(p => p.Map<CustomerResponse>(It.IsAny<Customer>())).Returns(customerResponse);

            var result = await _customerAppService.GetByCpfAsync(cpf);

            result.Should().NotBeNull();

            _customerServiceMock.Verify(p => p.GetByCpfAsync(It.IsAny<string>()), Times.Once);
            _mapperMock.Verify(p => p.Map<CustomerResponse>(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async void Should_Not_GetByCpf_When_Cpf_Dismatch()
        {
            var cpf = "42713070848";
            var customer = CustomerFixture.GenerateCustomerFixture();
            var customerResponse = CustomerResponseFixture.GenerateCustomerFixture();
            ArgumentNullException excnull = new();

            _customerServiceMock.Setup(p => p.GetByCpfAsync(It.IsAny<string>())).Throws(excnull);
            _mapperMock.Setup(p => p.Map<CustomerResponse>(It.IsAny<Customer>())).Returns(customerResponse);

            try
            {
                var result = await _customerAppService.GetByCpfAsync(cpf);
                result.Should().NotBeNull();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }

            _customerServiceMock.Verify(p => p.GetByCpfAsync(It.IsAny<string>()), Times.Once);
            _mapperMock.Verify(p => p.Map<CustomerResponse>(It.IsAny<Customer>()), Times.Never);

        }

        [Fact]
        public void Should_Update_Sucessfully()
        {
            var customer = CustomerFixture.GenerateCustomerFixture();
            var UpdateCustomer = UpdateCustomerFixture.GenerateCustomerFixture();

            _mapperMock.Setup(p => p.Map<Customer>(It.IsAny<UpdateCustomer>())).Returns(customer);
            _customerServiceMock.Setup(p => p.Update(It.IsAny<Customer>()));

            _customerAppService.Update(UpdateCustomer);

            _mapperMock.Verify(p => p.Map<Customer>(It.IsAny<UpdateCustomer>()), Times.Once);
            _customerServiceMock.Verify(p => p.Update(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public void Should_Not_Update_When_Id_Dismatch()
        {
            var customer = CustomerFixture.GenerateCustomerFixture();
            var UpdateCustomer = UpdateCustomerFixture.GenerateCustomerFixture();
            ArgumentNullException excnull = new();

            _mapperMock.Setup(p => p.Map<Customer>(It.IsAny<UpdateCustomer>())).Returns(customer);
            _customerServiceMock.Setup(p => p.Update(It.IsAny<Customer>())).Throws(excnull);

            try
            {
                _customerAppService.Update(UpdateCustomer);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }

            _mapperMock.Verify(p => p.Map<Customer>(It.IsAny<UpdateCustomer>()), Times.Once);
            _customerServiceMock.Verify(p => p.Update(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public void Should_Not_Update_When_Cpf_Or_Email_Already_Exists()
        {
            var customer = CustomerFixture.GenerateCustomerFixture();
            var UpdateCustomer = UpdateCustomerFixture.GenerateCustomerFixture();
            ArgumentException exc = new();

            _mapperMock.Setup(p => p.Map<Customer>(It.IsAny<UpdateCustomer>())).Returns(customer);
            _customerServiceMock.Setup(p => p.Update(It.IsAny<Customer>())).Throws(exc);

            try
            {
                _customerAppService.Update(UpdateCustomer);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }

            _mapperMock.Verify(p => p.Map<Customer>(It.IsAny<UpdateCustomer>()), Times.Once);
            _customerServiceMock.Verify(p => p.Update(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async void Should_GetById_SucessFully()
        {
            var id = 1;
            var customer = CustomerFixture.GenerateCustomerFixture();
            var customerResponse = CustomerResponseFixture.GenerateCustomerFixture();

            _customerServiceMock.Setup(p => p.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(customer);
            _mapperMock.Setup(p => p.Map<CustomerResponse>(It.IsAny<Customer>())).Returns(customerResponse);

            var result = await _customerAppService.GetByIdAsync(id);

            result.Should().NotBeNull();
            _customerServiceMock.Verify(p => p.GetByIdAsync(It.IsAny<long>()), Times.Once);

            _mapperMock.Verify(p => p.Map<CustomerResponse>(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async void Should_Not_GetById_When_Id_Dismatch()
        {
            var id = 1;
            var customer = CustomerFixture.GenerateCustomerFixture();
            var customerResponse = CustomerResponseFixture.GenerateCustomerFixture();
            ArgumentNullException excnull = new();

            _customerServiceMock.Setup(p => p.GetByIdAsync(It.IsAny<long>())).Throws(excnull);
            _mapperMock.Setup(p => p.Map<CustomerResponse>(It.IsAny<Customer>())).Returns(customerResponse);

            try
            {
                var result = await _customerAppService.GetByIdAsync(id);
                result.Should().NotBeNull();
            }
            catch (ArgumentNullException e)
            {

                Console.WriteLine(e.Message);
            }

            _customerServiceMock.Verify(p => p.GetByIdAsync(It.IsAny<long>()), Times.Once);
            _mapperMock.Verify(p => p.Map<CustomerResponse>(It.IsAny<Customer>()), Times.Never);
        }
    }
}
