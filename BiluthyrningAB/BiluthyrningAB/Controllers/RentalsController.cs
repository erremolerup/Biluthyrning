using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiluthyrningAB.Models;
using BiluthyrningAB.Data;
using Microsoft.EntityFrameworkCore;
using BiluthyrningAB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BiluthyrningAB.Controllers
{
    public class RentalsController : Controller
    {
        private readonly AppDbContext _context;

        public RentalsController(AppDbContext context) //Görs för att nå allting i AppDbContext, dvs. Rental, Car & Customer och för att slippa skapa nya instanser i varje funktion
        {
            _context = context;
        }

        // GET: Index för rentals
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rentals.Include(x => x.Car).Include(y => y.Customer).ToListAsync());
        }

        //GET: vyn där man skapar en bokning
        public IActionResult Create()
        {
            //CreateRentalVm rentalVm = new CreateRentalVm();

            //string[] arrayOfCarTypes = Enum.GetNames(typeof(CarType));

            //List<SelectListItem> list = new List<SelectListItem>();
            //foreach (var carType in arrayOfCarTypes)
            //{
            //    list.Add(new SelectListItem
            //    {
            //        Text = carType,
            //        Value = carType
            //    });
            //}
            //rentalVm.CarTypes = list;
            //rentalVm.Rental = new Rental();
            //return View(rentalVm);

            string[] arr = Enum.GetNames(typeof(CarType));

            var viewmodel = new CreateRentalVm();

            viewmodel.CarTypes = arr.Select(x => new SelectListItem
            {
                Text = x,
                Value = x
            });

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RentalId,StartDate,Customer,Car")] Rental rental)
        {
            //if (ModelState.IsValid)
            //{
            //    //    //Skapar ett rentalId och sparar bokning
            //    //    rentals.RentalId = Guid.NewGuid();
            //    _context.Add(rentals);

            //    //    //Skapar ett customerId och sparar kund
            //    //    rentals.Customer.CustomerId = Guid.NewGuid();
            //    //    _context.Add(rentals.Customer);

            //    //    //Skapar ett CarId och sparar bil
            //    //    rentals.Car.CarId = Guid.NewGuid();
            //    //    _context.Add(rentals.Car);

            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}

            //CreateRentalVm rentalVm = new CreateRentalVm();
            //string[] arrayOfCarTypes = Enum.GetNames(typeof(CarType));
            //var vm = new CreateRentalVm();

            //List<SelectListItem> list = new List<SelectListItem>();
            //foreach (var carType in arrayOfCarTypes)
            //{
            //    list.Add(new SelectListItem
            //    {
            //        Text = carType,
            //        Value = carType
            //    });
            //}
            //rentalVm.CarTypes = list;
            //return View(rentalVm);

            rental.Ongoing = true;
            rental.EndDate = rental.StartDate;

            if (ModelState.IsValid)
            {
                _context.Add(rental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var viewmodel = new CreateRentalVm();
            viewmodel.Rental = rental;
            return View(viewmodel);

        }

        //GET: Vy slutföra bokning
        public IActionResult FinishRental(Guid? id)
        {
            var rental = _context.Rentals.Include(x => x.Car).Single(x => x.RentalId == id);

            if (id == null)
                return NotFound();

            if (rental == null)
                return NotFound();

            return View(rental);
        }

        //POST: Slutföra bokning
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinishRental(Guid id, [Bind("RentalId,NumberOfKm,StartDate,EndDate,Car")] Rental rental)
        {
            if (id != rental.RentalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    rental.Ongoing = false;

                    rental.Car.NumberOfDrivenKm = rental.NewNumberKmDriven;
                        //Car.NumberOfDrivenKm + Convert.ToInt32(rental.NumberOfKm);

                    _context.Update(rental.Car);

                    _context.Update(rental);

                    await _context.SaveChangesAsync();
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

            var rental = _context.Rentals.Include(x => x.Car).Include(y => y.Customer).Single(x => x.RentalId == id);
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
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rental);
                    await _context.SaveChangesAsync();
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

            var rental = _context.Rentals.Include(x => x.Car).Include(y => y.Customer).Single(x => x.RentalId == id);
            if (rental == null)
                return NotFound();

            return View(rental);
        }

        //POST: ta bort en uthyrning
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET: vy för uthyrning - details
        public IActionResult Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var rental = _context.Rentals.Include(x => x.Car).Include(y => y.Customer).Single(x => x.RentalId == id);
            if (rental == null)
                return NotFound();

            return View(rental);
        }

        //Kollar på id ifall uthyrningen existerar
        private bool RentalExist(Guid id)
        {
            return _context.Rentals.Any(x => x.RentalId == id);
        }
    }
}
