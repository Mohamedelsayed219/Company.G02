using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G02.BLL.Interfaces;
using CompanyG02.DAL.Data.Contexts;
using CompanyG02.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.G02.BLL.repositories
{
    public class EmployeeRepository : GenericRepository<Employee> ,IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepository(CompanyDbContext context) : base(context) // ASK CLR Create Object From CompanyDbContext
        {
            _context = context;
        }

        public List<Employee> GetByName(string name)
        {
            return _context.Employees.Include(E =>E.Department). Where(E => E.Name.ToLower().Contains(name.ToLower())).ToList();
            
        }
    }
}
