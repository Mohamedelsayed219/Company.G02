using Company.G02.BLL.Interfaces;
using Company.G02.PL.Dtos;
using CompanyG02.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeerepository;

        // ASK CLR Create Object From IEmployeeRepository
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeerepository = employeeRepository;
        }

        [HttpGet] // GET: /Employee/Index
        public IActionResult Index()
        {

            var employees = _employeerepository.GetAll();

            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)  // Server Side Validation
            {
                var employee = new Employee()
                {
                   
                    Name = model.Name,
                    Address = model.Address,
                    Age = model.Age,
                    CreateAt = model.CreateAt,
                    HiringDate = model.HiringDate,
                    Email = model.Email,
                    IsActive = model.IsActive,
                    IsDelete = model.IsDelete,
                    Phone = model.Phone,
                    Salary = model.Salary,

                };
                var count = _employeerepository.Add(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invaild Id");

            var employee = _employeerepository.Get(id.Value);

            if (employee is null) return NotFound(new { StatusCode = 404, message = $"Employee With Id :{id} is not found" });

            return View(viewName, employee);
        }



        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //if (id is null) return BadRequest("Invaild Id");

            //var department = _departmentrepository.Get(id.Value);

            //if (department is null) return NotFound(new { StatusCode = 404, message = $"Department With Id :{id} is not found" });

            return Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee model)
        {

            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest();
                var count = _employeerepository.Update(model);

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, UpdateDepartmentDto model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var department = new Department()
        //        {
        //            Id = id,
        //            Name = model.Name,
        //            Code = model.Name,
        //            CreateAt = model.CreateAt
        //        };
        //        var count = _departmentrepository.Update(department);

        //        if (count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    return View(model);
        //}


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //if (id is null) return BadRequest("Invaild Id");

            //var department = _departmentrepository.Get(id.Value);

            //if (department is null) return NotFound(new { StatusCode = 404, message = $"Department With Id :{id} is not found" });


            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Employee model)
        {

            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest();
                var count = _employeerepository.Delete(model);

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        } 
    }
}
