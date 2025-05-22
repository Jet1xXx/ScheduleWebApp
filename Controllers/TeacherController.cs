using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScheduleWebApp.Models.Entities;
using ScheduleWebApp.Services;
using System.Text;

namespace ScheduleWebApp.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TeacherController> _logger;
        private readonly PhoneValidator _phoneValidator;

        public TeacherController(AppDbContext context, ILogger<TeacherController> logger, PhoneValidator phoneValidator)
        {
            _context = context;
            _logger = logger;
            _phoneValidator = phoneValidator;
        }

        public IActionResult Index()
        {
            try
            {
                List<Teacher.TeacherListItemDto> teachers = Teacher.GetAllTeachers(_context);
                _logger.LogInformation("Успешно загружено {Count} преподавателей", teachers.Count);
                return View("Index", teachers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке списка преподавателей");
                TempData["ErrorMessage"] = "Не удалось загрузить список преподавателей";
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Cities = Teacher.GetCitiesDropdown(_context);
            return View(new Teacher.TeacherEditDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Teacher.TeacherEditDto dto)
        {
            try
            {
                dto.Phone = _phoneValidator.Normalize(dto.Phone);

                if (!ModelState.IsValid)
                {
                    LogModelStateErrors();
                    ViewBag.Cities = Teacher.GetCitiesDropdown(_context);
                    return View(dto);
                }

                if (!_phoneValidator.IsValid(dto.Phone))
                {
                    ModelState.AddModelError("Phone", "Номер должен содержать минимум 10 цифр");
                    ViewBag.Cities = Teacher.GetCitiesDropdown(_context);
                    return View(dto);
                }

                Teacher.CreateTeacher(_context, dto);

                _logger.LogInformation("Создан преподаватель: {FullName} (ID: {Id})",
                    $"{dto.LastName} {dto.FirstName} {dto.MiddleName}".Trim(), dto.TeacherId);

                TempData["SuccessMessage"] = "Преподаватель успешно добавлен";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex.Message);
                ModelState.AddModelError("Email", ex.Message);
                ViewBag.Cities = Teacher.GetCitiesDropdown(_context);
                return View(dto);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при создании преподавателя. Сообщение: {Message}", ex.InnerException?.Message ?? ex.Message);
                ModelState.AddModelError("", "Не удалось сохранить данные. Проверьте корректность заполнения.");
                ViewBag.Cities = Teacher.GetCitiesDropdown(_context);
                return View(dto);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании преподавателя");
                TempData["ErrorMessage"] = "Произошла непредвиденная ошибка";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                Teacher.TeacherDetailsDto teacher = Teacher.GetTeacherDetails(_context, id);
                if (teacher == null)
                {
                    _logger.LogWarning("Преподаватель с ID: {Id} не найден", id);
                    return NotFound();
                }
                return View(teacher);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке данных преподавателя (ID: {Id})", id);
                TempData["ErrorMessage"] = "Не удалось загрузить данные преподавателя";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Edit(int id)
        {
            try
            {
                Teacher.TeacherEditDto teacher = Teacher.GetTeacherForEdit(_context, id);
                if (teacher == null)
                {
                    _logger.LogWarning("Преподаватель с ID: {Id} не найден", id);
                    return NotFound();
                }

                ViewBag.Cities = Teacher.GetCitiesDropdown(_context);
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
        public IActionResult Edit(int id, Teacher.TeacherEditDto dto)
        {
            if (id != dto.TeacherId)
            {
                _logger.LogWarning("Несоответствие ID: запрошенный {Id}, полученный {TeacherId}", id, dto.TeacherId);
                return NotFound();
            }

            try
            {
                dto.Phone = _phoneValidator.Normalize(dto.Phone);

                if (!ModelState.IsValid)
                {
                    LogModelStateErrors();
                    ViewBag.Cities = Teacher.GetCitiesDropdown(_context);
                    return View(dto);
                }

                if (!_phoneValidator.IsValid(dto.Phone))
                {
                    ModelState.AddModelError("Phone", "Номер должен содержать минимум 10 цифр");
                    ViewBag.Cities = Teacher.GetCitiesDropdown(_context);
                    return View(dto);
                }

                Teacher.UpdateTeacher(_context, dto);

                _logger.LogInformation("Обновлён преподаватель: {FullName} (ID: {Id})",
                    $"{dto.LastName} {dto.FirstName} {dto.MiddleName}".Trim(), dto.TeacherId);

                TempData["SuccessMessage"] = "Данные преподавателя обновлены";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!Teacher.TeacherExists(_context, id))
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

        public IActionResult Delete(int id)
        {
            try
            {
                Teacher.TeacherDetailsDto teacher = Teacher.GetTeacherDetails(_context, id);
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
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                bool result = Teacher.DeleteTeacher(_context, id);
                if (!result)
                {
                    _logger.LogWarning("Преподаватель с ID: {Id} не найден", id);
                    return NotFound();
                }

                _logger.LogInformation("Удален преподаватель с ID: {Id}", id);
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

        private void LogModelStateErrors()
        {
            StringBuilder errors = new StringBuilder("Ошибки валидации:\n");
            foreach (KeyValuePair<string, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry> entry in ModelState)
            {
                foreach (ModelError error in entry.Value.Errors)
                {
                    errors.AppendLine($"{entry.Key}: {error.ErrorMessage}");
                }
            }
            _logger.LogWarning(errors.ToString());
        }
    }
}
