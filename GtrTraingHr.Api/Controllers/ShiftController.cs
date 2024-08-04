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
    public class ShiftController : ControllerBase
    {
        private readonly IShiftRepo shiftRepo;

        public ShiftController(IShiftRepo shiftRepo)
        {
            this.shiftRepo = shiftRepo;
        }

        [HttpGet("DropDownData")]
        public async Task<ActionResult> DropDownData()
        {


            try
            {
                var selectlist = await shiftRepo.DropDownData(x => x.CompanyId == Request.Headers[ApplicationConst.CompanyHeaderName].ToString(), "Id", "ShiftName");
                return Ok(selectlist);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                var shiftList = await shiftRepo.Where(x => x.CompanyId == Request.Headers[ApplicationConst.CompanyHeaderName].ToString()).ToListAsync();
                return Ok(shiftList);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("GetbyId/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            try
            {
                var singleShift = await shiftRepo.Where(x => x.Id == id).SingleOrDefaultAsync();
                if (singleShift == null)
                    return NotFound();

                return Ok(singleShift);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody]Shift model)
        {
            try
            {
               
                await shiftRepo.Create(model);

                return StatusCode(StatusCodes.Status201Created, model);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

      
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(string id, Shift model)
        {
            try
            {
                var oldData = await shiftRepo.Single(x => x.Id == id);
                if (oldData == null)
                    return NotFound();
                oldData.ShiftOut = model.ShiftOut;
                oldData.ShiftIn = model.ShiftIn;
                oldData.ShiftLate = model.ShiftLate;
                oldData.ShiftName = model.ShiftName;
                oldData.CompanyId = Request.Headers[ApplicationConst.CompanyHeaderName].ToString();

                await shiftRepo.Update(oldData, id);

                return NoContent();
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
                var oldData = await shiftRepo.Where(x => x.Id == id).SingleOrDefaultAsync();
                if (oldData == null)
                    return NotFound();

                await shiftRepo.Delete(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
