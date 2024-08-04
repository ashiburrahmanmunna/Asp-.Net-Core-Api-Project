using GtrTraingHr.Models;
using GtrTraingHr.Models.Const;
using GtrTraingHr.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace GtrTraingHr.Services.Service
{
    public class EmployeeService:IEmployeeService
    {
        private readonly HttpClient client;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EmployeeService(HttpClient client, IHttpContextAccessor  httpContextAccessor)
        {
            this.client = client;
            this.httpContextAccessor = httpContextAccessor;
           var com= this.httpContextAccessor.HttpContext.Request.Cookies[ApplicationConst.CompanyHeaderName];
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(ApplicationConst.CompanyHeaderName, com);
        }
        public async Task<HttpResponseMessage> Create(Employee model)
        {
            var r = await client.PostAsJsonAsync("/api/Employee/", model);

            return r;
        }
        public async Task<SelectList> DropDownData()
        {

            return new SelectList(await GetAll(), "Id", "EmpName");
        }
        public async Task<string> Delete(string id)
        {
            var r = await client.DeleteAsync("/api/Employee/" + id);
            return await r.Content.ReadAsStringAsync();
        }

        public async Task<List<Employee>> GetAll()
        {
            var r = await client.GetFromJsonAsync<List<Employee>>("/api/Employee");
            return r;
        } 
        public async Task<List<EmpreportViewModel>> EmployeeReport(string dept,string desig)
        {
            var r = await client.GetFromJsonAsync<List<EmpreportViewModel>>($"/api/Employee/EmployeeReport?dept={dept}&desig={desig}");
            return r;
        }

        public async Task<Employee> GetById(int id)
        {
            var r = await client.GetFromJsonAsync<Employee>("/api/Employee/GetById/" + id);
            return r;
        }

        public async Task<Employee> GetById(string id)
        {
            var r = await client.GetFromJsonAsync<Employee>("/api/Employee/GetById/" + id);
            return r;
        }

       

        public async Task<HttpResponseMessage> Update(Employee model, object key)
        {
            var r = await client.PutAsJsonAsync("/api/Employee/" + key, model);
            return r;

        }

        //public async Task<string> GetEmployeeShiftId(string? empId)
        //{
        //    var r = await client.GetFromJsonAsync<string>("/api/Employee/GetEmployeeShiftId/" + empId);
        //    return r;
        //}
    }
}
