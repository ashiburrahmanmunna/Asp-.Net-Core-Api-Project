using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;

namespace GtrTraingHr.Data.Repository.Implemention
{
    public class EmployeeRepo : Repository<Employee>, IEmployeeRepo
    {
        public EmployeeRepo(GtrDbContext db) : base(db)
        {
        }
    }
}
