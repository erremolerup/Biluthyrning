﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BiluthyrningAB.Models;
using Microsoft.EntityFrameworkCore;
using BiluthyrningAB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using BiluthyrningAB.Services.Repositories;


namespace BiluthyrningAB.Controllers
{
    public class RentalsController : Controller
    {
        private readonly IRentalsRepository _rentalsRepository;
        private readonly ICarsRepository _carsRepository;
        private readonly ICustomersRepository _customersRepository;


        public RentalsController(IRentalsRepository rentalsRepository, ICarsRepository carsRepository, ICustomersRepository customersRepository)
        {
            _rentalsRepository = rentalsRepository;
            _carsRepository = carsRepository;
            _customersRepository = customersRepository;
        }
        // GET: Index för rentals
        public async Task<IActionResult> Index()
        {
            return View(await (Task.Run(() => _rentalsRepository.GetAllRentals())));
        }

        //GET: vyn där man skapar en bokning
        public IActionResult Create()
        {
            CreateRentalVm rentalVm = new CreateRentalVm();

            rentalVm.Cars = FillCarsListSelectListItems();

            rentalVm.Customers = FillCustomersListSelectListItems();

            return View(rentalVm);

        }

        public async Task<IActionResult> AllNonFinishedRentals()
        {
            var myTask = Task.Run(() => _rentalsRepository.GetRentalsByStatus(true));
            return View(await myTask);
        }


        public async Task<IActionResult> AllFinishedRentals()
        {
            var myTask = Task.Run(() => _rentalsRepository.GetRentalsByStatus(false));
            return View(await myTask);
        }

        public async Task<IActionResult> AllRentalsOnCustomer(Guid? CustomerId, Guid? RentalId)
        {
            var myTask = Task.Run(() => _rentalsRepository.GetRentalsForCertainCustomer(CustomerId));
            
                return View(await myTask);
            
        }

        private List<SelectListItem> FillCustomersListSelectListItems()
        {
            var customers = _customersRepository.GetAllCustomers();

            List<SelectListItem> listOfCustomers = new List<SelectListItem>();

            foreach (var customer in customers)
            {
                string wholeName = $"{customer.FirstName} {customer.LastName}";
                var x = new SelectListItem() { Text = wholeName, Value = customer.CustomerId.ToString() };
                listOfCustomers.Add(x);
            }
            return listOfCustomers;
        }

        private List<SelectListItem> FillCarsListSelectListItems()
        {
            var cars = _carsRepository.GetAllCars();

            List<SelectListItem> listOfCars = new List<SelectListItem>();

            foreach (var car in cars)
            {
                var y = new SelectListItem() { Text = car.NumberPlate, Value = car.CarId.ToString(), Group = new SelectListGroup { Name = car.CarType.ToString() } };
                listOfCars.Add(y);
            }
            return listOfCars;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RentalId,StartDate,CustomerId,CarId")] Rental rental)
        {
            rental.Ongoing = true;
            rental.EndDate = rental.StartDate;

            rental.Car = _carsRepository.GetCarById(rental.CarId);

            //om bilen är obokad sätter vi den till bokad
            if (rental.Car.Booked == false)
            {
                rental.Car.Booked = true;
            }
            else
            {
                ViewBag.Message = "Bilen är upptagen vid den önskade tidpunkten, vänligen välj en annan bil.";
                CreateRentalVm error_rentalVm = new CreateRentalVm();
                error_rentalVm.Cars = FillCarsListSelectListItems();
                error_rentalVm.Customers = FillCustomersListSelectListItems();
                return View(error_rentalVm);
            }
            if (ModelState.IsValid)
            {
                //lägger till bokning i systemet.
                rental.RentalId = Guid.NewGuid();
                _rentalsRepository.AddRental(rental);

                //uppdaterar till bokad bil
                _carsRepository.UpdateCar(rental.Car);

                return RedirectToAction(nameof(Index));
            }

            var viewmodel = new CreateRentalVm();
            viewmodel.Rental = rental;
            return View(viewmodel);

        }

        //GET: Vy slutföra bokning
        public IActionResult FinishRental(Guid? id)
        {
            var rental = _rentalsRepository.GetRentalById(id);

            if (id == null)
                return NotFound();

            if (rental == null)
                return NotFound();

            return View(rental);
        }

        //POST: Slutföra bokning
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinishRental(Guid id, [Bind("RentalId,NewNumberKmDriven,StartDate,EndDate,CarId,CustomerId")] Rental rental)
        {
            rental.Car = _carsRepository.GetCarById(rental.CarId);
            rental.Customer = _customersRepository.GetCustomerById(rental.CustomerId);

            if (id != rental.RentalId)
            {
                return NotFound();
            }
            if (rental.Price < 0)
            {
                ViewBag.Message = "Återlämningsdatum måste vara senare än uthämtningsdatum";
                return View(rental);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //avslutar bokning
                    rental.Ongoing = false;
                    //sätter bilen till ledig igen
                    rental.Car.Booked = false;
                    //uppdaterar bilens körda km
                    rental.Car.NumberOfDrivenKm = rental.NewNumberKmDriven;

                    _carsRepository.UpdateCar(rental.Car);

                    _rentalsRepository.UpdateRental(rental);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExist(rental.RentalId))
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
            return View(rental);
        }

        //GET: vy för att ändra bokning
        public IActionResult Edit(Guid? id)
        {

            if (id == null)
                return NotFound();

            var rental = _rentalsRepository.GetRentalById(id);
            if (rental == null)
                return NotFound();

            return View(rental);
        }

        //POST: Ändra bokning
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("RentalId,NumberOfKm,StartDate,EndDate,Car")] Rental rental)
        {
            if (id != rental.RentalId)
            {
                return NotFound();
            }
            if (rental.Price < 0)
            {
                ViewBag.Message = "Återlämningsdatum måste vara senare än uthämtningsdatum";
                return View(rental);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _rentalsRepository.UpdateRental(rental);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExist(rental.RentalId))
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
            return View(rental);
        }

        //GET: vy för att ta bort en uthyrning
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var rental = _rentalsRepository.GetRentalById(id);
            if (rental == null)
                return NotFound();

            return View(rental);
        }

        //POST: ta bort en uthyrning
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var rental = _rentalsRepository.GetRentalById(id);
            _rentalsRepository.RemoveRental(rental);
            return RedirectToAction(nameof(Index));
        }

        //GET: vy för uthyrning - details
        public IActionResult Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var rental = _rentalsRepository.GetRentalById(id);
            if (rental == null)
                return NotFound();

            return View(rental);
        }

        //Kollar på id ifall uthyrningen existerar
        private bool RentalExist(Guid id)
        {
            return _rentalsRepository.RentalExists(id);
        }
    }
}
