using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiluthyrningAB.Models;

namespace BiluthyrningAB.Services.Repositories
{
    public class CustomersRepository : ICustomersRepository
    {
        public void AddCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public bool CustomerExists(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomerById(Guid? id)
        {
            throw new NotImplementedException();
        }

        public void RemoveCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
