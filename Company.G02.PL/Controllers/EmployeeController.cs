using System.Reflection.Metadata;
using AutoMapper;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.repositories;
using Company.G02.PL.Dtos;
using Company.G02.PL.Helpers;
using CompanyG02.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeerepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        // ASK CLR Create Object From IEmployeeRepository
        public EmployeeController(
            //IEmployeeRepository employeeRepository,
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            //_employeerepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        [HttpGet] // GET: /Employee/Index
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                 employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else 
            {
                 employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
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
        public async Task<IActionResult> Create()
        {
            //var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            //ViewData["departments"] = departments;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)  // Server Side Validation
            {
                if (model.Image is not null) 
                {
                  model.ImageName =  DocumentSettings.UploadFile(model.Image, "images");

                }
                   var employee = _mapper.Map<Employee>(model);

                    await _unitOfWork.EmployeeRepository.AddAsync(employee);

                    var count = await _unitOfWork.CompleteAsync();

                    if (count > 0)
                    {
                        TempData["Message"] = "Employee is Created ! !";
                        return RedirectToAction(nameof(Index));
                    }
              
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invaild Id");

            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);

            if (employee is null) return NotFound(new { StatusCode = 404, message = $"employee With Id :{id} is not found" });

            var dto = _mapper.Map<CreateEmployeeDto>(employee);

            return View( dto);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id )
        {
            
            //var departments = await _unitOfWork.EmployeeRepository.GetAllAsync();
            //ViewData["departments"] = departments;

            return await Details(id,"Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto model )
        {

            if (ModelState.IsValid)
            {
               
                if (model.ImageName is not null && model.Image is not null) 
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");

                }

                if (model.Image is not null)
                {
                   model.ImageName= DocumentSettings.UploadFile(model.Image, "images");

                }


                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;

                _unitOfWork.EmployeeRepository.Update(employee);
                var count = await _unitOfWork.CompleteAsync();

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
        public async Task<IActionResult> Delete(int? id)
        {
          

            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, CreateEmployeeDto model)
        {

            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;

                _unitOfWork.EmployeeRepository.Delete(employee);
                var count = await _unitOfWork.CompleteAsync();

                if (count > 0)
                {
                    if (model.ImageName is not null)
                    {
                       DocumentSettings.DeleteFile(model.ImageName, "images");

                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
    }
}
