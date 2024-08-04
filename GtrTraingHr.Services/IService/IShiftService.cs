using GtrTraingHr.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtrTraingHr.Services.IService
{
    public interface IShiftService
    {
        Task<List<Shift>> GetAll();
        Task<Shift> GetById(int id);
        Task<Shift> GetById(string id);
        Task<HttpResponseMessage> Create(Shift model);
        Task<HttpResponseMessage> Update(Shift model, object key);
        Task<string> Delete(string id); Task<SelectList> DropDownData();
    }
}
