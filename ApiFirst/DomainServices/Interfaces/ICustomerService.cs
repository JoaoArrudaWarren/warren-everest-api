﻿using DomainModels.Models;

namespace DomainServices.Interfaces
{
    public interface ICustomerService

    {
        long Create(Customer model);

        void Update(int id, Customer model);

        void Delete(int id);

        List<Customer> GetAll();

        Customer? GetById(int id);

        void Modify(int id, string email);

        Customer? GetByCpf(string cpf);
    }
}