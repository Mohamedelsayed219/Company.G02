using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G02.BLL.Interfaces;
using CompanyG02.DAL.Data.Contexts;
using CompanyG02.DAL.Models;

namespace Company.G02.BLL.repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(CompanyDbContext context) : base(context) // ASK CLR Create Object From CompanyDbContext
        {

        }


    }
}
