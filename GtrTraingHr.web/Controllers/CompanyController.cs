using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;
using Microsoft.AspNetCore.Mvc;

namespace GtrTraingHr.web.Controllers
{
    public class CompanyController : Controller
    {
        
        private readonly ICompanyRepo companyRepo;

        public CompanyController(ICompanyRepo companyRepo)
        {
            
            this.companyRepo = companyRepo;
        }
        // GET: CompanyController
        public async Task<ActionResult> Index()
        {
            var companylist = await companyRepo.GetAllAsync();
            return View(companylist);
        }

        // GET: CompanyController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var singlecompany = await companyRepo.Single(x => x.Id == id);
            if (singlecompany == null)
                return NotFound();

            return View(singlecompany);
        }

        // GET: CompanyController/Create
        public async Task<ActionResult> CreateOrEdit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return View(new Company()
                {
                    Id = string.Empty
                });
            }
            else
            {
                var singlecompany = await companyRepo.Single(x => x.Id == id);
                if (singlecompany == null)
                    return NotFound();
                return View(singlecompany);
            }

          
        }

        // POST: CompanyController/Create
        [HttpPost]       
        public async Task<ActionResult> CreateOrEdit(Company model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id=Guid.NewGuid().ToString();
                    await companyRepo.Create(model);
                }
                else
                {
                    var olddata = await companyRepo.Single(x => x.Id == model.Id);
                    if (olddata == null)
                        return NotFound();
                    olddata.IsInactive = model.IsInactive;
                    olddata.ComName= model.ComName;
                    olddata.Basic=model.Basic;
                    olddata.HRent = model.HRent;
                    olddata.Medical=model.Medical;

                    await companyRepo.Update(olddata, model.Id);
                }
                

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(model);
            }
        }
        // POST: CompanyController/Delete/5
        [HttpPost]
       public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var olddata = await companyRepo.Single(x => x.Id == id);
                if (olddata == null)
                    return NotFound();

                await companyRepo.Delete(id);
                
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
