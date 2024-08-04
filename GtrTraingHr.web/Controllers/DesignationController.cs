using GtrTraingHr.Data;
using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace GtrTraingHr.web.Controllers
{
    public class DesignationController : Controller
    {
     
        private readonly IDesignationRepo designationRepo;

        public DesignationController(IDesignationRepo designationRepo)
        {
           
            this.designationRepo = designationRepo;
        }
        // GET: DesignationController
        public async Task<ActionResult> Index()
        {
            var companylist = await designationRepo.Where(x=>x.CompanyId== Request.Cookies["CompanyID"]).ToListAsync();
            return View(companylist);
        }

        // GET: DesignationController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var singlecompany = await designationRepo.Single(x => x.Id == id);
            if (singlecompany == null)
                return NotFound();

            return View(singlecompany);
        }

        // GET: DesignationController/Create
        public async Task<ActionResult> CreateOrEdit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return View(new Designation()
                {
                    Id = string.Empty
                });
            }
            else
            {
                var singlecompany = await designationRepo.Single(x => x.Id == id);
                if (singlecompany == null)
                    return NotFound();
                return View(singlecompany);
            }

          
        }

        // POST: DesignationController/Create
        [HttpPost]       
        public async Task<ActionResult> CreateOrEdit(Designation model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id=Guid.NewGuid().ToString();
                    model.CompanyId = Request.Cookies["CompanyID"];
                    await designationRepo.Create(model);
                }
                else
                {
                    var olddata = await designationRepo.Single(x => x.Id == model.Id);
                    if (olddata == null)
                        return NotFound();
                    olddata.CompanyId = Request.Cookies["CompanyID"];
                    olddata.DesigName = model.DesigName;

                    await designationRepo.Update(olddata, model.Id);
                }
               

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(model);
            }
        }
        // POST: DesignationController/Delete/5
        [HttpPost]
       public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var olddata = await designationRepo.Single(x => x.Id == id);
                if (olddata == null)
                    return NotFound();

               await designationRepo.Delete(id);
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
