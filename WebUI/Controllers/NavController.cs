using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {
        private ICarRepository repository;
        public NavController(ICarRepository repository)
        {
            this.repository = repository;
        }
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Cars
                .Select(car => car.Category)
                .Distinct()
                .OrderBy(x => x);

            return PartialView("Menu", categories);
        }
    }
}