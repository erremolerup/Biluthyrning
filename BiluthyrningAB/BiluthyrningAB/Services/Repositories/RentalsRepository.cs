using BiluthyrningAB.Data;
using BiluthyrningAB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BiluthyrningAB.Services.Repositories
{
    public class RentalsRepository : IRentalsRepository
    {
        private readonly AppDbContext _context;

        public RentalsRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddRental(Rental rental)
        {
            _context.Add(rental);
            _context.SaveChanges();

        }

        public IEnumerable<Rental> GetAllRentals()
        {
            return _context.Rentals.Include(x => x.Car).Include(y => y.Customer).ToList();
        }

        public Rental GetRentalById(Guid? id)
        {
            return _context.Rentals.Include(x => x.Customer).Include(x => x.Car)
                .FirstOrDefault(x => x.RentalId == id);
        }

        public IEnumerable<Rental> GetRentals()
        {
            return _context.Rentals.Include(x => x.Car).Include(y => y.Customer).ToList();
        }
        
        public IEnumerable<Rental> GetRentalsByStatus(bool status)
        {
            return _context.Rentals.Include(x => x.Car).Include(z => z.Customer).Where(x => x.Ongoing == status).ToList();
        }

        public IEnumerable<Rental> GetRentalsForCertainCustomer(Guid? CustomerId)
        {
            return _context.Rentals.Include(x => x.Car).Include(x => x.Customer).Where(x => x.Customer.CustomerId == CustomerId).ToList();
        }

        public void RemoveRental(Rental rental)
        {
            _context.Rentals.Remove(rental);
            _context.SaveChanges();

        }

        public bool RentalExists(Guid id)
        {
            return _context.Rentals.Any(e => e.RentalId == id);
        }

        public void UpdateRental(Rental rental)
        {
            _context.Update(rental);
            _context.SaveChanges();
        }
    }
}
