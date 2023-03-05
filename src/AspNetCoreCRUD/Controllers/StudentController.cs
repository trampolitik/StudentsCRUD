using AspNetCoreCRUD.Data;
using AspNetCoreCRUD.Models;
using AspNetCoreCRUD.Models.DbEntities;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreCRUD.Controllers
{
    /// <summary>Контроллер отвечающий за взаимодействие с базой данных и студентами</summar>
    public class StudentController : Controller
    {
        private readonly StudentDbContext _studentContext; //контекст базы данных 

        private readonly ILogger<StudentController> _logger; // логгирование

        public StudentController(ILogger<StudentController> logger, StudentDbContext studentContext)
        {
            _studentContext = studentContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var students = await _studentContext.Students.ToListAsync();
            List<StudentViewModel> studentList = new List<StudentViewModel>();

            if(students != null)
            { 
                foreach(var student in students)
                {
                    var studentViewModel = new StudentViewModel()
                    {
                        Id= student.Id,
                        Name= student.Name,
                        Age= student.Age,
                        Email= student.Email
                    };

                    studentList.Add(studentViewModel);
                }
                return View(studentList);
            }
            return View(studentList);  
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }
        /// <summary> Метод отвечающий за добавление студента в бд </summary>
        /// <param name="studentData">Принимаем модель студента</param>
        [HttpPost]
        public async Task<IActionResult> Add(StudentViewModel studentData)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var student = new Student()
                    {
                        Name = studentData.Name,
                        Age = studentData.Age,
                        Email = studentData.Email
                    };
                    await _studentContext.Students.AddAsync(student);
                    await _studentContext.SaveChangesAsync();
                    TempData["successMessage"] =  "Student added!";
                    _logger.LogInformation("Success add in database");
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Student isn't valid";                   
                    return View();
                }
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                _logger.LogInformation($"don't add database, error: {ex.Message}");
                return View();
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var student = await _studentContext.Students.SingleOrDefaultAsync(x => x.Id == id);
                if(student != null)
                {
                    var studentView = new StudentViewModel()
                    {
                        Id = student.Id,
                        Name = student.Name,
                        Age = student.Age,
                        Email = student.Email
                    };
                    return View(studentView);
                }
                else
                {
                    TempData["errorMessage"] = $"Student don't available with the id: {id}";
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
            
        }
        /// <summary> Метод отвечающий за обновление студента в бд </summary>
        /// <param name="studentModel">Принимаем модель студента</param>
        [HttpPost]
        public async Task<IActionResult> Edit(StudentViewModel studentModel)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var student = new Student()
                    {
                        Id = studentModel.Id,
                        Name = studentModel.Name,
                        Age = studentModel.Age,
                        Email = studentModel.Email
                    };
                    _studentContext.Students.Update(student);
                    await _studentContext.SaveChangesAsync();
                    TempData["successMessage"] = "Student update!";
                    _logger.LogInformation("Success update the database");
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Student isn't valid";
                    
                    return View();
                }
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                _logger.LogInformation($"Did not restore the database, error: {ex.Message}");
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var student = await _studentContext.Students.SingleOrDefaultAsync(x => x.Id == id);
                if(student != null)
                {
                    var studentView = new StudentViewModel()
                    {
                        Id = student.Id,
                        Name = student.Name,
                        Age = student.Age,
                        Email = student.Email
                    };
                    return View(studentView);
                }
                else
                {
                    TempData["errorMessage"] = $"Student don't available with the id: {id}";
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        /// <summary> Метод отвечающий за удаление студента в бд </summary>
        /// <param name="studentModel">Принимаем модель студента</param>
        [HttpPost]
        public async Task<IActionResult> Delete(StudentViewModel studentModel)
        {
            var student = await _studentContext.Students.SingleOrDefaultAsync(x => x.Id == studentModel.Id);
            if(student != null )
            {
                _studentContext.Students.Remove(student);
                 await _studentContext.SaveChangesAsync();
                TempData["successMessage"] = " Student delete!";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

    }
}
