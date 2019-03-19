﻿using BiluthyrningAB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiluthyrningAB.Services.Repositories
{
    public interface IRentalsRepository
    {
        IEnumerable<Rental> GetAllRentals();
        IEnumerable<Rental> GetRentalsForCertainCustomer(Guid? CustomerId);
        IEnumerable<Rental> GetRentalsByStatus(bool status);
         
        Rental GetRentalById(Guid? id);
        bool RentalExists(Guid id);

        void AddRental(Rental rental);
        void UpdateRental(Rental rental);
        void RemoveRental(Rental rental);
    }
}
