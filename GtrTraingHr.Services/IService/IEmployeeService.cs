using GtrTraingHr.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtrTraingHr.Services.IService
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAll();
        Task<List<EmpreportViewModel>> EmployeeReport(string dept, string desig);
        Task<Employee> GetById(int id);
        Task<Employee> GetById(string id);
        Task<HttpResponseMessage> Create(Employee model);
        Task<HttpResponseMessage> Update(Employee model, object key);
        Task<string> Delete(string id); Task<SelectList> DropDownData();
        //Task<string> GetEmployeeShiftId(string? empId);
    }
}
