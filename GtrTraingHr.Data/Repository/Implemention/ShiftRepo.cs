using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;

namespace GtrTraingHr.Data.Repository.Implemention
{
    public class ShiftRepo : Repository<Shift>, IShiftRepo
    {
        public ShiftRepo(GtrDbContext db) : base(db)
        {
        }
    }
}
