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
    public class CompanyService:ICompanyService
    {
        private readonly HttpClient client;
        //private readonly IHttpContextAccessor httpContextAccessor;

        public CompanyService(HttpClient client, IHttpContextAccessor  httpContextAccessor)
        {
            this.client = client;
           // this.httpContextAccessor = httpContextAccessor;
           //var com= this.httpContextAccessor.HttpContext.Request.Cookies[ApplicationConst.CompanyHeaderName];
          //client.DefaultRequestHeaders.Add(ApplicationConst.CompanyHeaderName, com);
        }
        public async Task<HttpResponseMessage> Create(Company model)
        {
            var r = await client.PostAsJsonAsync("/api/Company/", model);

            return r;
        }

        public async Task<string> Delete(string id)
        {
            var r = await client.DeleteAsync("/api/Company/" + id);
            return await r.Content.ReadAsStringAsync();
        }

        public async Task<SelectList> DropDownData()
        {
            var r = await client.GetFromJsonAsync<SelectList>("/api/Company/DropDownData");
            return r;
        }

        public async Task<List<Company>> GetAll()
        {
            var r = await client.GetFromJsonAsync<List<Company>>("/api/Company");
            return r;
        }

        public async Task<Company> GetById(int id)
        {
            var r = await client.GetFromJsonAsync<Company>("/api/Company/GetById/" + id);
            return r;
        }

        public async Task<Company> GetById(string id)
        {
            var r = await client.GetFromJsonAsync<Company>("/api/Company/GetById/" + id);
            return r;
        }

       

        public async Task<HttpResponseMessage> Update(Company model, object key)
        {
            var r = await client.PutAsJsonAsync("/api/Company/" + key, model);
            return r;

        }
    }
}
