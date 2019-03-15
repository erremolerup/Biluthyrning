using BiluthyrningAB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiluthyrningAB.Models;
using BiluthyrningAB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BiluthyrningAB.Controllers
{
    public class CarsController : Controller
    {
        private readonly AppDbContext _context;

        public CarsController(AppDbContext context)
        {
            _context = context;
        }

        //GET: vy för index
        public IActionResult Index()
        {
            return View(_context.Cars);
        }

        //GET: vy för create
        public IActionResult Create()
        {
            return View();
        }

        //POST: Skapa ny bil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("CarId,NumberPlate,CarType,NumberOfDrivenKm")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        //GET: vy för att ändra bil
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var car = _context.Cars.Single(x => x.CarId == id);
            if (car == null)
                return NotFound();

            return View(car);
        }


        //POST: Ändra bilinfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CarId,NumberPlate,CarType,NumberOfDrivenKm")] Car car)
        {
            if (id != car.CarId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExist(car.CarId))
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
            return View(car);
        }

        //GET: vy för att ta bort bil
        public async Task<IActionResult> Delete(Guid? id)
        {
            //return View();
            if (id == null)
                return NotFound();

            var rental = await _context.Cars
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (rental == null)
                return NotFound();

            return View(rental);
        }

        //POST: ta bort bil
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var car = await _context.Cars.FindAsync(id);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: vy för detaljer
        public IActionResult Details()
        {
            return View();
        }

        private bool CustomerExist(Guid id)
        {
            return _context.Cars.Any(x => x.CarId == id);
        }
    }
}
