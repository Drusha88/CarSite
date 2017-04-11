using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;
using System.Diagnostics;
using WebUI.Infrastructure;

namespace WebUI.Controllers
{
    [MyAuthentication]
    public class CarsController : Controller
    {
        public int pageSize = 5;
        private ICarRepository repository;
        public CarsController(ICarRepository repository)
        {
            this.repository = repository;
        }


        public ViewResult List(string category, int page = 1)
        {
            CarsListViewModel model = new CarsListViewModel
            {
                Cars = repository.Cars
                .Where(c => category == null || c.Category == category)
                .OrderBy(car => car.CarId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                        repository.Cars.Count() :
                        repository.Cars.Where(car => car.Category == category).Count()
                },
                CurrentCategory = category
            };
            Debug.WriteLine(model.Cars.ToString());
            return View(model);
        }

        public ViewResult Edit(int carId)
        {

            Car car = repository.Cars.FirstOrDefault(c => c.CarId == carId);
            return View(car);
        }


        [HttpPost]
        public ActionResult Edit(Car car)
        {
            if (ModelState.IsValid)
            {
                repository.SaveCar(car);
                return RedirectToAction("List");
            }
            else
            {
                return View(car);
            }
        }


	}
}