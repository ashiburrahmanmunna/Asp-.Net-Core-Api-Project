using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;

namespace GtrTraingHr.Data.Repository.Implemention
{
    public class DepartmentRepo : Repository<Department>, IDepartmentRepo
    {
        public DepartmentRepo(GtrDbContext db) : base(db)
        {
        }
    }
}
