using GtrTraingHr.Models;

namespace GtrTraingHr.Data.Repository.Interface
{
    public interface IAttendanceRepo:IRepository<Attendance>
    {
        Task<IEnumerable<AttendanceSummary>> AttendanceSummaries(string comId, int? month, int? year);
    }
}
