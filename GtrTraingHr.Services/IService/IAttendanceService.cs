using GtrTraingHr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtrTraingHr.Services.IService
{
    public interface IAttendanceService
    {
        Task<List<Attendance>> GetAll();
        Task<Attendance> GetById(int id);
        Task<Attendance> GetById(string id);
        Task<HttpResponseMessage> Create(AttendanceDTO model);
        Task<HttpResponseMessage> Update(AttendanceDTO model, object key);
        Task<string> Delete(string id);
        Task<IEnumerable<AttendanceSummary>> AttendanceSummaries(DateTime dateTime);
        Task<IEnumerable<Salary>> SalarySummary(DateTime dateTime);
        Task<HttpResponseMessage> SalaryPaid(string empId, int dtMonth, int dtYear);
        Task<List<SalaryReportViewModel>> SalarySummeryreport(string dept, string month, string year);
    }
}
