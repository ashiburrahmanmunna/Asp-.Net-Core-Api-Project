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
    public class DesignationController : ControllerBase
    {

        private readonly IDesignationRepo designationRepo;

        public DesignationController(IDesignationRepo designationRepo)
        {

            this.designationRepo = designationRepo;
        }
        [HttpGet("DropDownData")]
        public async Task<ActionResult> DropDownData()
        {


            try
            {
                var data = Request.Headers[ApplicationConst.CompanyHeaderName].ToString();
                var selectlist = await designationRepo.DropDownData(x => x.CompanyId == Request.Headers[ApplicationConst.CompanyHeaderName].ToString(), "Id", "DesigName");
                
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
                var companylist = await designationRepo.Where(x => x.CompanyId == Request.Headers[ApplicationConst.CompanyHeaderName].ToString()).ToListAsync();
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
                var singlecompany = await designationRepo.Single(x => x.Id == id);
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
        public async Task<ActionResult> Create([FromBody] Designation model)
        {
            try
            {
                model.CompanyId = Request.Headers[ApplicationConst.CompanyHeaderName].ToString();
                await designationRepo.Create(model);
                return Ok("Save Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, Designation model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest("Id Mismatch");
                }
                var olddata = await designationRepo.Single(x => x.Id == model.Id);
                if (olddata == null)
                    return NotFound();
                model.CompanyId = Request.Headers[ApplicationConst.CompanyHeaderName].ToString();
                olddata.DesigName = model.DesigName;

                await designationRepo.Update(olddata, model.Id);

                return Ok("Save Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        // POST: DesignationController/Delete/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var olddata = await designationRepo.Single(x => x.Id == id);
                if (olddata == null)
                    return NotFound();

                await designationRepo.Delete(id);
                return Ok("Delete Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
