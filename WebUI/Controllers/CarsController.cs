using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.Infrastructure;
using System.IO;

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


        public ViewResult List(string category, string search, int page = 1)
        {

            if (string.IsNullOrEmpty(search))
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
                return View(model);
            }
            else
            {
                CarsListViewModel model = new CarsListViewModel 
                {
                    Cars = repository.Cars
                    .Where( c => c.Name.ToLower().Contains(search.ToLower()))
                    .OrderBy(car => car.CarId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),

                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = repository.Cars.Where(c => c.Name.ToLower().Contains(search.ToLower())).Count()
                    },
                    CurrentCategory = null
                };
                TempData["message"] = string.Format("Поиск \"{0}\", результатов: {1}.", search, model.Cars.Count());
                return View(model);
            }
        }

        public ViewResult Edit(int carId)
        {

            Car car = repository.Cars.FirstOrDefault(c => c.CarId == carId);
            if (car != null)
                return View(car);
            else
                return View();
        }


        [HttpPost]
        public ActionResult Edit(Car car, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    car.ImageMimeType = image.ContentType;
                    car.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(car.ImageData, 0, image.ContentLength);
                }
                else
                {
                    Car tempCar = repository.Cars.FirstOrDefault(c => c.CarId == car.CarId);
                    if(tempCar != null)
                    {
                        if(tempCar.ImageData != null)
                        {
                            car.ImageData = tempCar.ImageData;
                            car.ImageMimeType = tempCar.ImageMimeType;
                        }
                    }
                }
                repository.SaveCar(car);
                TempData["message"] = string.Format("Запись \"{0}\" успешно изменена.", car.Name);
                return RedirectToAction("List");
            }
            else
            {
                return View(car);
            }
        }

        public ViewResult Create()
        {
            return View(new Car());
        }

        [HttpPost]
        public ActionResult Create(Car car, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    car.ImageMimeType = image.ContentType;
                    car.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(car.ImageData, 0, image.ContentLength);
                }
                repository.SaveCar(car);
                TempData["message"] = string.Format("Запись \"{0}\" успешно сохранена.", car.Name);
                return RedirectToAction("List");
            }
            else
            {
                return View(car);
            }
        }

        public PartialViewResult Search()
        {
            string str = string.Empty;
            return PartialView(str);
        }

        public ActionResult Delete(int carId)
        {

            Car deletedCar = repository.DeleteCar(carId);
            if (deletedCar != null)
            {
                TempData["message"] = string.Format("Запись \"{0}\" была удалена", deletedCar.Name);
            }
            return RedirectToAction("List");
        }

        public FileContentResult GetImage(int carId)
        {
            Car car = repository.Cars
                .FirstOrDefault(g => g.CarId == carId);

            if (car != null)
            {
                return File(car.ImageData, car.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
	}
    
}