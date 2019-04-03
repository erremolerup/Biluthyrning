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
using BiluthyrningAB.Services.Repositories;

namespace BiluthyrningAB.Controllers
{
    public class CarsController : Controller
    {
        //private readonly AppDbContext _context;

        //public CarsController(AppDbContext context)
        //{
        //    _context = context;
        //}
        private readonly ICarsRepository _carsRepository;

        public CarsController(IRentalsRepository rentalsRepository, ICarsRepository carsRepository, ICustomersRepository customersRepository)
        {
            _carsRepository = carsRepository;
        }

        //GET: vy för index
        public async Task<IActionResult> Index()
        {
            //return View(_context.Cars);
            return View(await(Task.Run(() => _carsRepository.GetAllCars())));

        }

        //GET: vy för create
        public IActionResult Create()
        {
            //return View();
            CarVm carTypeVm = new CarVm();

            carTypeVm.CarTypes = GetCarTypeToSelectList();

            return View(carTypeVm);
        }

        private List<SelectListItem> GetCarTypeToSelectList()
        {
            string[] carTypesArray = Enum.GetNames(typeof(CarType));

            List<SelectListItem> listOfCarTypes = new List<SelectListItem>();

            foreach (var car in carTypesArray)
            {
                var y = new SelectListItem() { Text = car, Value = car };
                listOfCarTypes.Add(y);
            }
            return listOfCarTypes;
        }

        //POST: Skapa ny bil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("CarId,NumberPlate,CarType,NumberOfDrivenKm")] Car car)
        {
            if (ModelState.IsValid)
            {
                _carsRepository.AddCar(car);
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        //GET: vy för att ändra bil
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var car = _carsRepository.GetCarById(id);
            if (car == null)
                return NotFound();

            CarVm carTypeVm = new CarVm();

            carTypeVm.CarTypes = GetCarTypeToSelectList();
            carTypeVm.Car = car;

            return View(carTypeVm);
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
                    _carsRepository.UpdateCar(car);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExist(car.CarId))
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
            CarVm carTypeVm = new CarVm();

            carTypeVm.CarTypes = GetCarTypeToSelectList();
            carTypeVm.Car = car;
            return View(carTypeVm);
        }

        //GET: vy för att ta bort bil
        public async Task<IActionResult> Delete(Guid? id)
        {
            //return View();
            if (id == null)
                return NotFound();

            var car = _carsRepository.GetCarById(id);
            if (car == null)
                return NotFound();

            return View(car);
        }

        //POST: ta bort bil
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var car = _carsRepository.GetCarById(id);
            _carsRepository.RemoveCar(car);
            return RedirectToAction(nameof(Index));
        }

        // GET: vy för detaljer
        public IActionResult Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var car = _carsRepository.GetCarById(id);

            if (car == null)
                return NotFound();

            return View(car);
        }

        private bool CarExist(Guid id)
        {
            return _carsRepository.CarExists(id);
        }
    }
}
