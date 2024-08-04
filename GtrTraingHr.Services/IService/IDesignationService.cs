using GtrTraingHr.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtrTraingHr.Services.IService
{
    public interface IDesignationService
    {
        Task<List<Designation>> GetAll();
        Task<Designation> GetById(int id);
        Task<Designation> GetById(string id);
        Task<HttpResponseMessage> Create(Designation model);
        Task<HttpResponseMessage> Update(Designation model, object key);
        Task<string> Delete(string id); Task<SelectList> DropDownData();
    }
}
