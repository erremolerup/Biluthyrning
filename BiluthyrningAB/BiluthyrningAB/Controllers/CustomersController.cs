using BiluthyrningAB.Data;
using BiluthyrningAB.Models;
using BiluthyrningAB.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB.Controllers
{
    public class CustomersController : Controller
    {
        //private readonly AppDbContext _context;

        //public CustomersController(AppDbContext context)
        //{
        //    _context = context;
        //}
        private readonly ICustomersRepository _customersRepository;

        public CustomersController(IRentalsRepository rentalsRepository, ICarsRepository carsRepository, IEntityFrameworkRepository entityFrameworkRepository, ICustomersRepository customersRepository)
        {
            _customersRepository = customersRepository;
        }

        //GET: vy för index
        public async Task<IActionResult> Index()
        {
            return View(await(Task.Run(() => _customersRepository.GetAllCustomers())));
        }

        //GET: vy för create
        public IActionResult Create()
        {
            return View();
        }

        //POST: Skapa ny kund
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,SocSecNumber,FirstName,LastName")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.CustomerId = Guid.NewGuid();
                _customersRepository.AddCustomer(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        //GET: vy för att ändra bokning
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var customer = _customersRepository.GetCustomerById(id);
            if (customer == null)
                return NotFound();

            return View(customer);
        }


        //POST: Ändra kundinfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CustomerId,FirstName,LastName,SocSecNumber")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _customersRepository.UpdateCustomer(customer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExist(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        //GET: vy för att ta bort kund
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = _customersRepository.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        //POST: ta bort kund
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var customer = _customersRepository.GetCustomerById(id);
            _customersRepository.RemoveCustomer(customer);
            return RedirectToAction(nameof(Index));
        }

        // GET: vy för detaljer
        public IActionResult Details()
        {
            return View();
        }

        private bool CustomerExist(Guid id)
        {
            return _customersRepository.CustomerExists(id);

        }
    }
}
