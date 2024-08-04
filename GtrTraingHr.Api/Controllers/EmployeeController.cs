using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;
using GtrTraingHr.Models.Const;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GtrTraingHr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepo employeeRepo;
        private readonly IDesignationRepo designationRepo;
        private readonly IDepartmentRepo departmentRepo;
        private readonly IShiftRepo shiftRepo;
        private readonly ICompanyRepo companyRepo;

        public EmployeeController(IEmployeeRepo employeeRepo,
            IDesignationRepo designationRepo,
            IDepartmentRepo departmentRepo,
            IShiftRepo shiftRepo,
            ICompanyRepo companyRepo)
        {
            this.employeeRepo = employeeRepo;
            this.designationRepo = designationRepo;
            this.departmentRepo = departmentRepo;
            this.shiftRepo = shiftRepo;
            this.companyRepo = companyRepo;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllData()
        {
            try
            {
                var employeeList = await employeeRepo
                    .Where(x => x.CompanyId == Request.Headers[ApplicationConst.CompanyHeaderName].ToString())
                    .ToListAsync();

                return Ok(employeeList);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            try
            {
                var singleEmployee = await employeeRepo.Single(x => x.Id == id);
                if (singleEmployee == null)
                    return NotFound();

                return Ok(singleEmployee);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(Employee model)
        {
            var salary = await companyRepo.Single(x => x.Id == Request.Headers[ApplicationConst.CompanyHeaderName].ToString());
            try
            {
              //  model.Id = Guid.NewGuid().ToString();
                model.Basic = model.Gross * (salary.Basic / 100);
                model.HRent = model.Gross * (salary.HRent / 100);
                model.Medical = model.Gross * (salary.Medical / 100);
                model.Others = model.Gross - (model.Basic + model.HRent + model.Medical);
                await employeeRepo.Create(model);

                return Ok("Save Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, Employee model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest("Id Mismatch");
                }
                var salary = await companyRepo.Single(x => x.Id == Request.Headers[ApplicationConst.CompanyHeaderName].ToString());
                              
                model.Basic = model.Gross * (salary.Basic / 100);
                model.HRent = model.Gross * (salary.HRent / 100);
                model.Medical = model.Gross * (salary.Medical / 100);
                model.Others = model.Gross - (model.Basic + model.HRent + model.Medical);

                await employeeRepo.Update(model, id);

                return Ok("Update Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var employee = await employeeRepo.Single(x => x.Id == id);
                if (employee == null)
                    return NotFound();

                await employeeRepo.Delete(id);

                return Ok("Delete Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("EmployeeReport")]
        public async Task<ActionResult> EmployeeReport(string? dept, string? desig)
        {
            try
            {
                var employeeList = employeeRepo.Where(x => x.CompanyId == Request.Headers[ApplicationConst.CompanyHeaderName].ToString());
                if (!string.IsNullOrEmpty(dept))
                {
                    employeeList = employeeList.Where(x => x.DeptId == dept);
                }
                if (!string.IsNullOrEmpty(desig))
                {
                    employeeList = employeeList.Where(x => x.DesigId == desig);
                }
                var employeeReports = await employeeList.Select(x => new EmpreportViewModel
                {
                    EmpName = x.EmpName,
                    ComName = x.Company.ComName,
                    DeptName = x.Dept.DeptName,
                    Gross = x.Gross,
                    Basic = x.Basic,
                    HRent = x.HRent,
                    Medical = x.Medical,
                    Others = x.Others,
                    dtJoin = x.dtJoin.ToShortDateString(),
                    Gender = x.Gender,
                    ShiftName = x.Shift.ShiftName,
                    jd = x.dtJoin
                }).ToListAsync();

                return Ok(employeeReports);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
