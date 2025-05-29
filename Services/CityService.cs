using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScheduleWebApp.Models.Entities;

namespace ScheduleWebApp.Services
{
    public class CityService
    {
        private readonly AppDbContext _context;

        public CityService(AppDbContext context)
        {
            _context = context;
        }

        // Метод для получения списка городов в формате SelectListItem (для DropDown)
        public List<SelectListItem> GetCitiesDropdown()
        {
            return _context.Cities
                .AsNoTracking()
                .OrderBy(c => c.CityName)
                .Select(c => new SelectListItem
                {
                    Value = c.CityId.ToString(),
                    Text = c.CityName
                })
                .ToList();
        }

        // Метод для вывода списка городов (например, в таблицу)
        public List<City> GetAllCities()
        {
            return _context.Cities
                .AsNoTracking()
                .OrderBy(c => c.CityName)
                .ToList();
        }

        // Метод для получения одного города по ID
        public City? GetCityById(int id)
        {
            return _context.Cities.Find(id);
        }
    }
}
