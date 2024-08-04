using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;
using Microsoft.EntityFrameworkCore;

namespace GtrTraingHr.Data.Repository.Implemention
{
    public class AttendanceRepo : Repository<Attendance>, IAttendanceRepo
    {
        public AttendanceRepo(GtrDbContext db) : base(db)
        {
        }
      public async  Task<IEnumerable<AttendanceSummary>> AttendanceSummaries(string comId,int? month,int? year) {
           return await db.AttendanceSummaries.Include(x => x.Emp)
                .Where(x => x.CompanyId == comId && x.dtMonth == month && x.dtYear == year)
                .ToListAsync();
        }
    }
}
