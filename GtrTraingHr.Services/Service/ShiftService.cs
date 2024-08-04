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
    public class ShiftService:IShiftService
    {
        private readonly HttpClient client;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ShiftService(HttpClient client, IHttpContextAccessor  httpContextAccessor)
        {
            this.client = client;
            this.httpContextAccessor = httpContextAccessor;
           var com= this.httpContextAccessor.HttpContext.Request.Cookies[ApplicationConst.CompanyHeaderName];
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(ApplicationConst.CompanyHeaderName, com);
        }
        public async Task<HttpResponseMessage> Create(Shift model)
        {
            var r = await client.PostAsJsonAsync("/api/Shift/", model);

            return r;
        }
        public async Task<SelectList> DropDownData()
        {

            return new SelectList(await GetAll(), "Id", "ShiftName");
        }
        public async Task<string> Delete(string id)
        {
            var r = await client.DeleteAsync("/api/Shift/" + id);
            return await r.Content.ReadAsStringAsync();
        }

        public async Task<List<Shift>> GetAll()
        {
            var r = await client.GetFromJsonAsync<List<Shift>>("/api/Shift");
            return r;
        }

        public async Task<Shift> GetById(int id)
        {
            var r = await client.GetFromJsonAsync<Shift>("/api/Shift/GetById/" + id);
            return r;
        }

        public async Task<Shift> GetById(string id)
        {
            var r = await client.GetFromJsonAsync<Shift>("/api/Shift/GetById/" + id);
            return r;
        }

       

        public async Task<HttpResponseMessage> Update(Shift model, object key)
        {
            var r = await client.PutAsJsonAsync("/api/Shift/" + key, model);
            return r;

        }
    }
}
