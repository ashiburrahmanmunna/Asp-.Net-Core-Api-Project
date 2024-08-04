using GtrTraingHr.Models;
using GtrTraingHr.Models.Const;
using GtrTraingHr.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace GtrTraingHr.Services.Service
{
    public class DepartmentService:IDepartmentService
    {
        private readonly HttpClient client;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DepartmentService(HttpClient client, IHttpContextAccessor  httpContextAccessor)
        {
            this.client = client;
            this.httpContextAccessor = httpContextAccessor;
           var com= this.httpContextAccessor.HttpContext.Request.Cookies[ApplicationConst.CompanyHeaderName]; 
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(ApplicationConst.CompanyHeaderName, com);
        }
        public async Task<HttpResponseMessage> Create(Department model)
        {
            var r = await client.PostAsJsonAsync("/api/Department/", model);

            return r;
        }
        public async Task<SelectList> DropDownData()
        {

            return new SelectList(await GetAll(), "Id", "DeptName");
        }
        public async Task<string> Delete(string id)
        {
            var r = await client.DeleteAsync("/api/Department/" + id);
            return await r.Content.ReadAsStringAsync();
        }

        public async Task<List<Department>> GetAll()
        {
            var r = await client.GetFromJsonAsync<List<Department>>("/api/Department");
            return r;
        }

        public async Task<Department> GetById(int id)
        {
            var r = await client.GetFromJsonAsync<Department>("/api/Department/GetById/" + id);
            return r;
        }

        public async Task<Department> GetById(string id)
        {
            var r = await client.GetFromJsonAsync<Department>("/api/Department/GetById/" + id);
            return r;
        }

       

        public async Task<HttpResponseMessage> Update(Department model, object key)
        {
            var r = await client.PutAsJsonAsync("/api/Department/" + key, model);
            return r;

        }
    }
}
