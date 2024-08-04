using GtrTraingHr.Models;
using GtrTraingHr.Models.Const;
using GtrTraingHr.Services.IService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace GtrTraingHr.Services.Service
{
    public class AttendanceService:IAttendanceService
    {
        private readonly HttpClient client;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AttendanceService(HttpClient client, IHttpContextAccessor  httpContextAccessor)
        {
            this.client = client;
            this.httpContextAccessor = httpContextAccessor;
           var com= this.httpContextAccessor.HttpContext.Request.Cookies[ApplicationConst.CompanyHeaderName]; client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(ApplicationConst.CompanyHeaderName, com);
        }

        public async Task<IEnumerable<AttendanceSummary>> AttendanceSummaries(DateTime dateTime)
        {
            var r = await client.GetFromJsonAsync<List<AttendanceSummary>>($"/api/Attendance/AttendenceSummery?dateTime={dateTime}");
            return r;
        }

        public async Task<HttpResponseMessage> Create(AttendanceDTO model)
        {
            var r = await client.PostAsJsonAsync("/api/Attendance/", model);

            return r;
        }

        public async Task<string> Delete(string id)
        {
            var r = await client.DeleteAsync("/api/Attendance/" + id);
            return await r.Content.ReadAsStringAsync();
        }

        public async Task<List<Attendance>> GetAll()
        {
            var r = await client.GetFromJsonAsync<List<Attendance>>("/api/Attendance");
            return r;
        }

        public async Task<Attendance> GetById(int id)
        {
            var r = await client.GetFromJsonAsync<Attendance>("/api/Attendance/GetById/" + id);
            return r;
        }

        public async Task<Attendance> GetById(string id)
        {
            var r = await client.GetFromJsonAsync<Attendance>("/api/Attendance/GetById/" + id);
            return r;
        }

        public async Task<HttpResponseMessage> SalaryPaid(string empId, int dtMonth, int dtYear)
        {
            var r = await client.GetAsync($"/api/Attendance/SalaryPaid?empId={empId}&dtMonth={dtMonth}&dtYear={dtYear}");
            return r;
        }

        public async Task<IEnumerable<Salary>> SalarySummary(DateTime dateTime)
        {
            var r = await client.GetFromJsonAsync<List<Salary>>($"/api/Attendance/SalarySummary?dateTime={dateTime}");
            return r;
        }

        public async Task<List<SalaryReportViewModel>> SalarySummeryreport(string dept, string month, string year)
        {
            var r = await client.GetFromJsonAsync<List<SalaryReportViewModel>>($"/api/Report/SalarySummeryreport?dept={dept}&month={month}&year={year}");
            return r;
        }

        public async Task<HttpResponseMessage> Update(AttendanceDTO model, object key)
        {
            var r = await client.PutAsJsonAsync("/api/Attendance/" + key, model);
            return r;

        }
    }
}
