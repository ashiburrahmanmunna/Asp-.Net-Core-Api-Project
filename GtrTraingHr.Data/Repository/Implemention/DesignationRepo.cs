using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;

namespace GtrTraingHr.Data.Repository.Implemention
{
    public class DesignationRepo : Repository<Designation>, IDesignationRepo
    {
        public DesignationRepo(GtrDbContext db) : base(db)
        {
        }
    }
}
