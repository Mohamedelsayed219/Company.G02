using AutoMapper;
using Company.G02.BLL.Interfaces;
using Company.G02.PL.Dtos;
using CompanyG02.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeerepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        // ASK CLR Create Object From IEmployeeRepository
        public EmployeeController(
            IEmployeeRepository employeeRepository,
            //IDepartmentRepository departmentRepository,
            IMapper mapper
            )
        {
            _employeerepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        [HttpGet] // GET: /Employee/Index
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                 employees = _employeerepository.GetAll();
            }
            else 
            {
                 employees = _employeerepository.GetByName(SearchInput);
            }


            // Dictionary : 3 Property
            // 1. ViewData : Transfer Extra Information From Controller (Action) To View
            //ViewData["Message"] = "Hello From ViewData";


            // 2. ViewBag  : Transfer Extra Information From Controller (Action) To View
            //ViewBag.Message = new {Message = "Hello From ViewBag"};


            // 3. TempData

            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //var department = _departmentRepository.GetAll();
            //ViewData["department"] = department;

            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)  // Server Side Validation
            {
                try 
                {
                    // Manual Mapping
                    //var employee = new Employee()
                    //{

                    //    Name = model.Name,
                    //    Address = model.Address,
                    //    Age = model.Age,
                    //    CreateAt = model.CreateAt,
                    //    HiringDate = model.HiringDate,
                    //    Email = model.Email,
                    //    IsActive = model.IsActive,
                    //    IsDelete = model.IsDelete,
                    //    Phone = model.Phone,
                    //    Salary = model.Salary,
                    //    DepartmentId = model.DepartmentId,

                    //};
                    var employee = _mapper.Map<Employee>(model);
                    var count = _employeerepository.Add(employee);
                    if (count > 0)
                    {
                        TempData["Message"] = "Employee is Created ! !";
                        return RedirectToAction(nameof(Index));
                    }
                } 
                catch (Exception ex) 
                {
                    ModelState.AddModelError("",ex.Message);
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
            //var department = _departmentRepository.GetAll();
            //ViewData["department"] = department;
            if (id is null) return BadRequest("Invaild Id");

            var employee = _employeerepository.Get(id.Value);

            if (employee is null) return NotFound(new { StatusCode = 404, message = $"Employee With Id :{id} is not found" });


            var dto = _mapper.Map<CreateEmployeeDto>(employee);
            var employeeDto = new CreateEmployeeDto()
            {

                EmpName = employee.Name,
                Address = employee.Address,
                Age = employee.Age,
                CreateAt = employee.CreateAt,
                HiringDate = employee.HiringDate,
                Email = employee.Email,
                IsActive = employee.IsActive,
                IsDelete = employee.IsDelete,
                Phone = employee.Phone,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId,

            };
            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee model)
        {

            if (ModelState.IsValid)
            {
                //if (id != model.Id) return BadRequest();
                var employee = new Employee()
                {
                    Id = id,
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
                    DepartmentId = model.DepartmentId,

                };
                var count = _employeerepository.Update(employee);

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
