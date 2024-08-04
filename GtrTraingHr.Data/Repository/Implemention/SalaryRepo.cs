using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;

namespace GtrTraingHr.Data.Repository.Implemention
{
    public class SalaryRepo : Repository<Salary>, ISalaryRepo
    {
        public SalaryRepo(GtrDbContext db) : base(db)
        {
        }
    }
}
