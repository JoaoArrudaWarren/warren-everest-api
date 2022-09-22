﻿using DomainModels.Models;
using DomainServices.Interfaces;

namespace DomainServices.Repositories
{
    public class CustomerService : ICustomerService
    {
        private readonly List<Customer> _customers = new();

        public long Create(Customer model)
        {
            model.Id = _customers.LastOrDefault()?.Id + 1 ?? 0;

            if (_customers.Any(customer => customer.Cpf == model.Cpf)) throw new ArgumentException($"Já existe usuário com o CPF {model.Cpf}");
            if (_customers.Any(customer => customer.Email == model.Email)) throw new ArgumentException($"Já existe usuário com o CPF {model.Email}");


            _customers.Add(model);
            return model.Id;
        }

        public bool ExistsUpdate(long id, Customer model)
        {
            var response = _customers.Any(customer => (customer.Cpf == model.Cpf || customer.Email == model.Email) && customer.Id != id);
            return response;
        }

        public bool Exists(Customer model)
        {
            var response = _customers.Any(customer => customer.Cpf == model.Cpf || customer.Email == model.Email);
            return response;
        }

        public void Delete(int id)
        {
            int index = _customers.FindIndex(customer => customer.Id == id);

            if (index == -1) throw new ArgumentNullException($"Cliente não encontrado para o id: {id}");

            _customers.RemoveAt(index);
        }

        public List<Customer> GetAll()
        {
            return _customers;
        }

        public Customer? GetByCpf(string cpf)
        {
            cpf = cpf.Trim().Replace(".", "").Replace("-", "");
            var response = _customers.FirstOrDefault(customer => customer.Cpf == cpf);
            if (response is null) throw new ArgumentNullException($"$Não foi encontrado Customer para o CPF: {cpf}");
            return response;
        }

        public void Update(int id, Customer model)
        {

            int index = _customers.FindIndex(customer => customer.Id == id);

            if (index == -1) throw new ArgumentNullException($"$Não foi encontrado Customer para o Id: {id}");

            if (_customers.Any(customer => customer.Cpf == model.Cpf)) throw new ArgumentException($"Já existe usuário com o CPF {model.Cpf}");
            if (_customers.Any(customer => customer.Email == model.Email)) throw new ArgumentException($"Já existe usuário com o CPF {model.Email}");

            model.Id = _customers[index].Id;
            _customers[index] = model;
        }

        public Customer? GetById(int id)
        {
            var response = _customers.FirstOrDefault(customer => customer.Id == id);
            if (response is null) throw new ArgumentNullException($"$Não foi encontrado Customer para o Id: {id}");
            return response;
        }

        public void Modify(int id, string email)
        {
            int index = _customers.FindIndex(customer => customer.Id == id);

            if (index == -1) throw new ArgumentNullException($"Não foi encontrado Customer para o Id: {id}");

            else if (_customers.Any(customer => customer.Email == email)) throw new ArgumentException("Já existe usuário com o E-mail ou CPF digitados"); ;

            _customers[index].Id = id;
            _customers[index].Email = email;
        }
    }
}