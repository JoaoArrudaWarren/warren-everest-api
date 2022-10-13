using AppModels.AppModels;
using System.Collections.Generic;

namespace AppServices.Interfaces
{
    public interface ICustomerAppService
    {
        long Create(CreateCustomer model);
        void Update(UpdateCustomer model);
        void Delete(long id);
        IEnumerable<CustomerResponse> GetAll();
        CustomerResponse GetById(long id);
        CustomerResponse GetByCpf(string cpf);
    }
}
