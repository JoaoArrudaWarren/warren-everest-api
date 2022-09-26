﻿using AppModels.AppModels;

namespace AppServices.Interfaces
{
    public interface ICustomerAppService
    {
        long Create(CreateCustomerDTO model);
        void Update(int id, UpdateCustomerDTO model);
        void Delete(int id);
        List<CustomerResponseDTO> GetAll();
        CustomerResponseDTO GetById(int id);
        void Modify(int id, UpdateCustomerDTO model);
        CustomerResponseDTO GetByCpf(string cpf);
    }
}
