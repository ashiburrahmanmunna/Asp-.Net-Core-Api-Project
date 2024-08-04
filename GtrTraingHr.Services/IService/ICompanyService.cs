using GtrTraingHr.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GtrTraingHr.Services.IService
{
    public interface ICompanyService
    {
        Task<List<Company>> GetAll();
        Task<Company> GetById(int id);
        Task<Company> GetById(string id);
        Task<HttpResponseMessage> Create(Company model);
        Task<HttpResponseMessage> Update(Company model, object key);
        Task<string> Delete(string id);
        Task<SelectList> DropDownData();
    }
}
