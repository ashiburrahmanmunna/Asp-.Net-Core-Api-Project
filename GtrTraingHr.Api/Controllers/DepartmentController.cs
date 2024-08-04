using GtrTraingHr.Data.Repository.Implemention;
using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;
using GtrTraingHr.Models.Const;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GtrTraingHr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepo departmentRepo;

        public DepartmentController(IDepartmentRepo departmentRepo)
        {
            this.departmentRepo = departmentRepo;
        }
        [HttpGet("DropDownData")]
        public async Task<ActionResult> DropDownData()
        {


            try
            {
                var selectlist = await departmentRepo.DropDownData(x=>x.CompanyId== Request.Headers[ApplicationConst.CompanyHeaderName].ToString(), "Id", "DeptName");
                return Ok(selectlist);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }
        [HttpGet]
        public async Task<ActionResult> GetAllData()
        {
            try
            {
                var companylist = await departmentRepo
                    .Where(x => x.CompanyId == Request.Headers[ApplicationConst.CompanyHeaderName].ToString())
                    .ToListAsync();

                return Ok(companylist);
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
                var singlecompany = await departmentRepo.Single(x => x.Id == id);
                if (singlecompany == null)
                    return NotFound();

                return Ok(singlecompany);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(Department model)
        {
            try
            {
                //model.CompanyId = Request.Headers[ApplicationConst.CompanyHeaderName].ToString();
                await departmentRepo.Create(model);
                return Ok("Save Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, Department model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest("Id Mismatch");
                }

                var olddata = await departmentRepo.Single(x => x.Id == model.Id);
                if (olddata == null)
                    return NotFound();

                olddata.CompanyId = Request.Headers[ApplicationConst.CompanyHeaderName].ToString();
                olddata.DeptName = model.DeptName;

                await departmentRepo.Update(olddata, model.Id);

                return Ok("Save Successfully");
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
                var olddata = await departmentRepo.Single(x => x.Id == id);
                if (olddata == null)
                    return NotFound();

                await departmentRepo.Delete(id);
                return Ok("Delete Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
