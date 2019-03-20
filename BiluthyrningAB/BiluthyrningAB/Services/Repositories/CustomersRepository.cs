using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiluthyrningAB.Data;
using BiluthyrningAB.Models;

namespace BiluthyrningAB.Services.Repositories
{
    public class CustomersRepository : ICustomersRepository
    {
        private readonly AppDbContext _context;

        public CustomersRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddCustomer(Customer customer)
        {
            _context.Add(customer);
            _context.SaveChanges();

        }

        public bool CustomerExists(Guid id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }

        public IEnumerable<Customer> GetAllCustomers() 
        {
            return _context.Customers.ToList();
        }

        public Customer GetCustomerById(Guid? id)
        {
            return _context.Customers
                .Single(x => x.CustomerId == id);
        }

        public void RemoveCustomer(Customer customer)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();


        }

        public void UpdateCustomer(Customer customer)
        {
            _context.Update(customer);
            _context.SaveChanges();

        }
    }
}