using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        private readonly TeacherService _teacherService;
        private readonly CityService _cityService;

        public TeacherController(AppDbContext context,
                                  ILogger<TeacherController> logger,
                                  TeacherService teacherService,
                                  CityService cityService)
        {
            _context = context;
            _logger = logger;
            _teacherService = teacherService;
            _cityService = cityService;
        }

        public IActionResult Index()
        {
            try
            {
                System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
                List<Teacher.TeacherListItemDto> teacherList = _teacherService.GetAllTeachers();
                stopwatch.Stop();

                _logger.LogInformation("Успешно загружено {Count} преподавателей за {ElapsedMilliseconds} мс",
                    teacherList.Count, stopwatch.ElapsedMilliseconds);

                ViewBag.ElapsedTime = stopwatch.ElapsedMilliseconds;
                return View("Index", teacherList);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ошибка при загрузке списка преподавателей");
                TempData["ErrorMessage"] = "Не удалось загрузить список преподавателей";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Cities = _cityService.GetCitiesDropdown();
            return View(new Teacher.TeacherDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Teacher.TeacherDto dto)
        {
            try
            {
                dto.Phone = PhoneValidator.Normalize(dto.Phone);

                if (!ModelState.IsValid)
                {
                    LogModelStateErrors();
                    ViewBag.Cities = _cityService.GetCitiesDropdown();
                    return View(dto);
                }

                if (!PhoneValidator.IsValid(dto.Phone))
                {
                    ModelState.AddModelError("Phone", "Номер должен содержать минимум 10 цифр");
                    ViewBag.Cities = _cityService.GetCitiesDropdown();
                    return View(dto);
                }

                _teacherService.CreateTeacher(dto);

                _logger.LogInformation("Создан преподаватель: {FullName} (ID: {Id})",
                    $"{dto.LastName} {dto.FirstName} {dto.MiddleName}".Trim(), dto.TeacherId);

                TempData["SuccessMessage"] = "Преподаватель успешно добавлен";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException exception)
            {
                _logger.LogWarning(exception.Message);
                ModelState.AddModelError("Email", exception.Message);
                ViewBag.Cities = _cityService.GetCitiesDropdown();
                return View(dto);
            }
            catch (DbUpdateException exception)
            {
                _logger.LogError(exception, "Ошибка базы данных при создании преподавателя. Сообщение: {Message}", exception.InnerException?.Message ?? exception.Message);
                ModelState.AddModelError("", "Не удалось сохранить данные. Проверьте корректность заполнения.");
                ViewBag.Cities = _cityService.GetCitiesDropdown();
                return View(dto);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ошибка при создании преподавателя");
                TempData["ErrorMessage"] = "Произошла непредвиденная ошибка";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                Teacher.TeacherDto teacher = _teacherService.GetTeacher(id);
                if (teacher == null)
                {
                    _logger.LogWarning("Преподаватель с ID: {Id} не найден", id);
                    return NotFound();
                }
                return View(teacher);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ошибка при загрузке данных преподавателя (ID: {Id})", id);
                TempData["ErrorMessage"] = "Не удалось загрузить данные преподавателя";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Edit(int id)
        {
            try
            {
                Teacher.TeacherDto teacher = _teacherService.GetTeacher(id);
                if (teacher == null)
                {
                    _logger.LogWarning("Преподаватель с ID: {Id} не найден", id);
                    return NotFound();
                }

                ViewBag.Cities = _cityService.GetCitiesDropdown();
                return View(teacher);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ошибка при загрузке формы редактирования (ID: {Id})", id);
                TempData["ErrorMessage"] = "Не удалось загрузить данные преподавателя";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Teacher.TeacherDto dto)
        {
            if (id != dto.TeacherId)
            {
                _logger.LogWarning("Несоответствие ID: запрошенный {Id}, полученный {TeacherId}", id, dto.TeacherId);
                return NotFound();
            }

            try
            {
                dto.Phone = PhoneValidator.Normalize(dto.Phone);

                if (!ModelState.IsValid)
                {
                    LogModelStateErrors();
                    ViewBag.Cities = _cityService.GetCitiesDropdown();
                    return View(dto);
                }

                if (!PhoneValidator.IsValid(dto.Phone))
                {
                    ModelState.AddModelError("Phone", "Номер должен содержать минимум 10 цифр");
                    ViewBag.Cities = _cityService.GetCitiesDropdown();
                    return View(dto);
                }


                _teacherService.UpdateTeacher(dto);

                _logger.LogInformation("Обновлён преподаватель: {FullName} (ID: {Id})",
                    $"{dto.LastName} {dto.FirstName} {dto.MiddleName}".Trim(), dto.TeacherId);

                TempData["SuccessMessage"] = "Данные преподавателя обновлены";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException exception)
            {
                _logger.LogError(exception, "Ошибка конкурентного доступа (ID: {Id})", id);
                TempData["ErrorMessage"] = "Данные были изменены другим пользователем. Пожалуйста, обновите страницу.";
                return RedirectToAction(nameof(Edit), new { id });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ошибка при обновлении (ID: {Id})", id);
                TempData["ErrorMessage"] = "Произошла ошибка при обновлении данных";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                Teacher.TeacherDto teacher = _teacherService.GetTeacher(id);
                if (teacher == null)
                {
                    _logger.LogWarning("Преподаватель с ID: {Id} не найден", id);
                    return NotFound();
                }

                return View(teacher);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ошибка при загрузке формы удаления (ID: {Id})", id);
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
                bool deleted = _teacherService.DeleteTeacher(id);
                if (!deleted)
                {
                    _logger.LogWarning("Преподаватель с ID: {Id} не найден", id);
                    return NotFound();
                }

                _logger.LogInformation("Удален преподаватель с ID: {Id}", id);
                TempData["SuccessMessage"] = "Преподаватель успешно удален";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException exception)
            {
                _logger.LogError(exception, "Ошибка базы данных при удалении (ID: {Id})", id);
                TempData["ErrorMessage"] = "Не удалось удалить преподавателя. Возможно, есть связанные данные.";
                return RedirectToAction(nameof(Delete), new { id });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ошибка при удалении преподавателя (ID: {Id})", id);
                TempData["ErrorMessage"] = "Произошла непредвиденная ошибка";
                return RedirectToAction(nameof(Index));
            }
        }

        private void LogModelStateErrors()
        {
            StringBuilder errorMessageBuilder = new StringBuilder("Ошибки валидации:\n");
            foreach (KeyValuePair<string, ModelStateEntry> modelStateEntry in ModelState)
            {
                foreach (ModelError modelError in modelStateEntry.Value.Errors)
                {
                    errorMessageBuilder.AppendLine($"{modelStateEntry.Key}: {modelError.ErrorMessage}");
                }
            }
            _logger.LogWarning(errorMessageBuilder.ToString());
        }
    }
}
