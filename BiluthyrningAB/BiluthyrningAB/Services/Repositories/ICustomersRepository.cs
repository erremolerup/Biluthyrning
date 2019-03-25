using BiluthyrningAB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiluthyrningAB.Services.Repositories
{
    public interface ICustomersRepository
    {
        IEnumerable<Customer> GetAllCustomers();

        Customer GetCustomerById(Guid? id);
        bool CustomerExists(Guid id);
        //bool CustomerExistOnName(string FirstName, string LastName);

        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void RemoveCustomer(Customer customer);
    }
}
