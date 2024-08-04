
using GtrTraingHr.Models;
using GtrTraingHr.Models.Const;
using GtrTraingHr.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GtrTraingHr.ConsumeAPI.Controllers
{
    public class ShiftController : Controller
    {

        private readonly IShiftService shiftService;

        public ShiftController(IShiftService shiftService)
        {

            this.shiftService = shiftService;
        }
        // GET: ShiftController
        public async Task<ActionResult> Index()
        {
            var companylist = await shiftService.GetAll();
            return View(companylist);
        }

        // GET: ShiftController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var singlecompany = await shiftService.GetById(id);
            if (singlecompany == null)
                return NotFound();

            return View(singlecompany);
        }

        // GET: ShiftController/Create
        public async Task<ActionResult> CreateOrEdit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return View(new Shift()
                {
                    Id = string.Empty
                });
            }
            else
            {
                var singlecompany = await shiftService.GetById(id);
                if (singlecompany == null)
                    return NotFound();
                return View(singlecompany);
            }


        }

        // POST: ShiftController/Create
        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(Shift model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString();
                    model.CompanyId = Request.Cookies[ApplicationConst.CompanyHeaderName].ToString();
                    await shiftService.Create(model);
                }
                else
                {
                    var olddata = await shiftService.GetById(model.Id);
                    if (olddata == null)
                        return NotFound();
                    olddata.ShiftOut = model.ShiftOut;
                    olddata.ShiftIn = model.ShiftIn;
                    olddata.ShiftLate = model.ShiftLate;
                    olddata.ShiftName = model.ShiftName;
                    olddata.CompanyId = Request.Cookies[ApplicationConst.CompanyHeaderName].ToString();

                    await shiftService.Update(olddata, model.Id);

                }


                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(model);
            }
        }
        // POST: ShiftController/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                

                await shiftService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View();
            }
        }
    }
}
