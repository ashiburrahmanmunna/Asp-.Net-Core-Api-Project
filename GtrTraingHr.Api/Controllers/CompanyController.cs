using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;
using GtrTraingHr.Models.Const;
using Microsoft.AspNetCore.Mvc;

namespace GtrTraingHr.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {

        private readonly ICompanyRepo companyRepo;

        public CompanyController(ICompanyRepo companyRepo)
        {

            this.companyRepo = companyRepo;
        }
        [HttpGet("DropDownData")]
        public async Task<ActionResult> DropDownData()
        {


            try
            {
                var companylist = await companyRepo.DropDownData(null,"Id", "ComName");
                return Ok(companylist);
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
                var companylist = await companyRepo.GetAllAsync();
                return Ok(companylist);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            try
            {
                var singlecompany = await companyRepo.Single(x => x.Id == id);
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
        public async Task<ActionResult> Create(Company model)
        {
            try
            {

                await companyRepo.Create(model);
                return Ok("Save Successfully");
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, Company model)
        {
            try
            {


                var olddata = await companyRepo.Single(x => x.Id == model.Id);
                if (olddata == null)
                    return NotFound();
                olddata.IsInactive = model.IsInactive;
                olddata.ComName = model.ComName;
                olddata.Basic = model.Basic;
                olddata.HRent = model.HRent;
                olddata.Medical = model.Medical;

                await companyRepo.Update(olddata, model.Id);



                return Ok("Update Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        // POST: CompanyController/Delete/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var olddata = await companyRepo.Single(x => x.Id == id);
                if (olddata == null)
                    return NotFound();

                await companyRepo.Delete(id);

                return Ok("Delete Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
