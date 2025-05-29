using Microsoft.AspNetCore.Mvc;
using ScheduleWebApp.Services;

namespace ScheduleWebApp.Controllers
{
    public class CityController : Controller
    {
        private readonly CityService _cityService;

        public CityController(CityService cityService)
        {
            _cityService = cityService;
        }

        // Вывод списка городов (например, на страницу /City/Index)
        public IActionResult Index()
        {
            var cities = _cityService.GetAllCities();
            return View(cities);
        }
    }
}
