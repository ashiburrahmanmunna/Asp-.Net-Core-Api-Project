using GtrTraingHr.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtrTraingHr.Services.IService
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAll();
        Task<Department> GetById(int id);
        Task<Department> GetById(string id);
        Task<HttpResponseMessage> Create(Department model);
        Task<HttpResponseMessage> Update(Department model, object key);
        Task<string> Delete(string id); Task<SelectList> DropDownData();
    }
}
