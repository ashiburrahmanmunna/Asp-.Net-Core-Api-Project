using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GtrTraingHr.web.Controllers
{
    public class ShiftController : Controller
    {
      
        private readonly IShiftRepo shiftRepo;

        public ShiftController(IShiftRepo shiftRepo)
        {
           
            this.shiftRepo = shiftRepo;
        }
        // GET: ShiftController
        public async Task<ActionResult> Index()
        {
            var companylist = await shiftRepo.Where(x=>x.CompanyId== Request.Cookies["CompanyID"]).ToListAsync();
            return View(companylist);
        }

        // GET: ShiftController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var singlecompany = await shiftRepo.Where(x => x.Id == id).SingleOrDefaultAsync();
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
                var singlecompany = await shiftRepo.Single(x => x.Id == id);
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
                    model.CompanyId= Request.Cookies["CompanyId"];
                    await shiftRepo.Create(model);
                }
                else
                {
                    var olddata = await shiftRepo.Single(x => x.Id == model.Id);
                    if (olddata == null)
                        return NotFound();
                    olddata.ShiftOut = model.ShiftOut;
                    olddata.ShiftIn = model.ShiftIn;
                    olddata.ShiftLate = model.ShiftLate;
                    olddata.ShiftName = model.ShiftName;
                    olddata.CompanyId = Request.Cookies["CompanyId"];

                    await shiftRepo.Update(olddata, model.Id);

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
                var olddata = await shiftRepo.Where(x => x.Id == id).SingleOrDefaultAsync();
                if (olddata == null)
                    return NotFound();

                await shiftRepo.Delete(id);               
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
