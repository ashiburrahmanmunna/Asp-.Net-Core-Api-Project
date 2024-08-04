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
    public class DesignationService:IDesignationService
    {
        private readonly HttpClient client;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DesignationService(HttpClient client, IHttpContextAccessor  httpContextAccessor)
        {
            this.client = client;
            this.httpContextAccessor = httpContextAccessor;
           var com= this.httpContextAccessor.HttpContext.Request.Cookies[ApplicationConst.CompanyHeaderName];
            client.DefaultRequestHeaders.Clear();
          client.DefaultRequestHeaders.Add(ApplicationConst.CompanyHeaderName, com);
        }
        public async Task<HttpResponseMessage> Create(Designation model)
        {
            var r = await client.PostAsJsonAsync("/api/Designation/", model);

            return r;
        }
        public async Task<SelectList> DropDownData()
        {
           
            return new SelectList(await GetAll(), "Id","DesigName");
        }
        public async Task<string> Delete(string id)
        {
            var r = await client.DeleteAsync("/api/Designation/" + id);
            return await r.Content.ReadAsStringAsync();
        }

        public async Task<List<Designation>> GetAll()
        {
            var r = await client.GetFromJsonAsync<List<Designation>>("/api/Designation");
            return r;
        }

        public async Task<Designation> GetById(int id)
        {
            var r = await client.GetFromJsonAsync<Designation>("/api/Designation/GetById/" + id);
            return r;
        }

        public async Task<Designation> GetById(string id)
        {
            var r = await client.GetFromJsonAsync<Designation>("/api/Designation/GetById/" + id);
            return r;
        }

       

        public async Task<HttpResponseMessage> Update(Designation model, object key)
        {
            var r = await client.PutAsJsonAsync("/api/Designation/" + key, model);
            return r;

        }
    }
}
