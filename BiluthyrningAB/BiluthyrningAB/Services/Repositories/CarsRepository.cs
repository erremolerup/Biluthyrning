using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiluthyrningAB.Data;
using BiluthyrningAB.Models;

namespace BiluthyrningAB.Services.Repositories
{
    public class CarsRepository : ICarsRepository
    {
        private readonly AppDbContext _context;

        public CarsRepository(AppDbContext context)
        {
            _context = context;
        }
        public void AddCar(Car car)
        {
            _context.Add(car);
            _context.SaveChanges();

        }

        public bool CarExists(Guid id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }

        public IEnumerable<Car> GetAllCars()
        {
            return _context.Cars.ToList();
        }

        public Car GetCarById(Guid? id)
        {
            //return _context.Cars.Single(x => x.CarId == id);
            return _context.Cars.FirstOrDefault(x => x.CarId == id);
        }

        public IEnumerable<Car> GetCarsDependingOnBookingStatus(bool status)
        {
            return _context.Cars.Where(x => x.Booked == status).ToList();
        }

        public void RemoveCar(Car car)
        {
            _context.Cars.Remove(car);
            _context.SaveChanges();

        }

        public void UpdateCar(Car car)
        {
            _context.Update(car);

            _context.SaveChanges();

        }
    }
}
