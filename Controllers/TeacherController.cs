using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleWebApp.Data;
using ScheduleWebApp.Models;
using System.Text;

namespace ScheduleWebApp.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TeacherController> _logger;

        public TeacherController(AppDbContext context, ILogger<TeacherController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var teachers = _context.Teachers
                    .AsNoTracking()
                    .Include(t => t.City)
                    .OrderBy(t => t.LastName)
                    .ThenBy(t => t.FirstName)
                    .ToList();

                _logger.LogInformation("Успешно загружено {Count} преподавателей", teachers.Count);
                return View(teachers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке списка преподавателей");
                TempData["ErrorMessage"] = "Не удалось загрузить список преподавателей";
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Create()
        {
            LoadCities();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Teacher teacher)
        {
            try
            {
                NormalizePhoneNumber(teacher);

                if (!ModelState.IsValid)
                {
                    LogModelStateErrors();
                    LoadCities();
                    return View(teacher);
                }

                if (!ValidatePhoneNumber(teacher.Phone))
                {
                    ModelState.AddModelError("Phone", "Номер должен содержать минимум 10 цифр");
                    LoadCities();
                    return View(teacher);
                }

                _context.Teachers.Add(teacher);
                _context.SaveChanges();

                _logger.LogInformation("Создан преподаватель: {FullName} (ID: {Id})",
                    GetFullName(teacher), teacher.TeacherId);

                TempData["SuccessMessage"] = "Преподаватель успешно добавлен";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при создании преподавателя");
                ModelState.AddModelError("", "Не удалось сохранить данные. Возможно, преподаватель с таким email уже существует.");
                LoadCities();
                return View(teacher);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании преподавателя");
                TempData["ErrorMessage"] = "Произошла непредвиденная ошибка";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Edit(long id)
        {
            try
            {
                var teacher = _context.Teachers.Find(id);
                if (teacher == null)
                {
                    _logger.LogWarning("Преподаватель с ID: {Id} не найден", id);
                    return NotFound();
                }

                LoadCities();
                return View(teacher);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке формы редактирования (ID: {Id})", id);
                TempData["ErrorMessage"] = "Не удалось загрузить данные преподавателя";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, Teacher teacher)
        {
            if (id != teacher.TeacherId)
            {
                _logger.LogWarning("Несоответствие ID: запрошенный {Id}, полученный {TeacherId}",
                    id, teacher.TeacherId);
                return NotFound();
            }

            try
            {
                NormalizePhoneNumber(teacher);

                if (!ModelState.IsValid)
                {
                    LogModelStateErrors();
                    LoadCities();
                    return View(teacher);
                }

                if (!ValidatePhoneNumber(teacher.Phone))
                {
                    ModelState.AddModelError("Phone", "Номер должен содержать минимум 10 цифр");
                    LoadCities();
                    return View(teacher);
                }

                _context.Update(teacher);
                _context.SaveChanges();

                _logger.LogInformation("Обновлен преподаватель: {FullName} (ID: {Id})",
                    GetFullName(teacher), teacher.TeacherId);

                TempData["SuccessMessage"] = "Данные преподавателя обновлены";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TeacherExists(id))
                {
                    _logger.LogWarning("Преподаватель с ID: {Id} не найден", id);
                    return NotFound();
                }

                _logger.LogError(ex, "Ошибка конкурентного доступа при редактировании (ID: {Id})", id);
                TempData["ErrorMessage"] = "Данные были изменены другим пользователем. Пожалуйста, обновите страницу.";
                return RedirectToAction(nameof(Edit), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении преподавателя (ID: {Id})", id);
                TempData["ErrorMessage"] = "Произошла ошибка при обновлении данных";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Delete(long id)
        {
            try
            {
                var teacher = _context.Teachers
                    .Include(t => t.City)
                    .FirstOrDefault(t => t.TeacherId == id);

                if (teacher == null)
                {
                    _logger.LogWarning("Преподаватель с ID: {Id} не найден", id);
                    return NotFound();
                }

                return View(teacher);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке формы удаления (ID: {Id})", id);
                TempData["ErrorMessage"] = "Не удалось загрузить данные преподавателя";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            try
            {
                var teacher = _context.Teachers.Find(id);
                if (teacher == null)
                {
                    _logger.LogWarning("Преподаватель с ID: {Id} не найден", id);
                    return NotFound();
                }

                _context.Teachers.Remove(teacher);
                _context.SaveChanges();

                _logger.LogInformation("Удален преподаватель: {FullName} (ID: {Id})",
                    GetFullName(teacher), id);

                TempData["SuccessMessage"] = "Преподаватель успешно удален";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при удалении (ID: {Id})", id);
                TempData["ErrorMessage"] = "Не удалось удалить преподавателя. Возможно, есть связанные данные.";
                return RedirectToAction(nameof(Delete), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении преподавателя (ID: {Id})", id);
                TempData["ErrorMessage"] = "Произошла непредвиденная ошибка";
                return RedirectToAction(nameof(Index));
            }
        }

        #region Вспомогательные методы

        private void LoadCities()
        {
            ViewBag.Cities = _context.Cities
                .AsNoTracking()
                .OrderBy(c => c.CityName)
                .ToList();
        }

        private bool TeacherExists(long id)
        {
            return _context.Teachers.Any(e => e.TeacherId == id);
        }

        private void NormalizePhoneNumber(Teacher teacher)
        {
            if (!string.IsNullOrEmpty(teacher.Phone))
            {
                teacher.Phone = new string(teacher.Phone
                    .Where(c => char.IsDigit(c) || c == '+')
                    .ToArray());
            }
        }

        private bool ValidatePhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return true;
            var digitCount = phone.Count(char.IsDigit);
            return digitCount >= 10;
        }

        private void LogModelStateErrors()
        {
            var errors = new StringBuilder("Ошибки валидации:\n");
            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    errors.AppendLine($"{entry.Key}: {error.ErrorMessage}");
                }
            }
            _logger.LogWarning(errors.ToString());
        }

        private string GetFullName(Teacher teacher)
        {
            return $"{teacher.LastName} {teacher.FirstName} {teacher.MiddleName}".Trim();
        }

        #endregion
    }
}