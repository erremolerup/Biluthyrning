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
            throw new NotImplementedException();
        }

        public bool CarExists(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Car> GetAllCars()
        {
            throw new NotImplementedException();
        }

        public Car GetCarById(Guid? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Car> GetCarsDependingOnBookingStatus(bool status)
        {
            throw new NotImplementedException();
        }

        public void RemoveCar(Car car)
        {
            throw new NotImplementedException();
        }

        public void UpdateCar(Car car)
        {
            throw new NotImplementedException();
        }
    }
}
