using GtrTraingHr.Models;
using GtrTraingHr.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace GtrTraingHr.ConsumeAPI.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }
        // GET: CompanyController
        public async Task<ActionResult> Index()
        {
            var companylist = await companyService.GetAll();
            return View(companylist);
        }

        // GET: CompanyController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var singlecompany = await companyService.GetById(id);
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
                var singlecompany = await companyService.GetById(id);
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
                    await companyService.Create(model);
                }
                else
                {
                   
                    await companyService.Update(model,model.Id);
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
               
                await companyService.Delete(id);
                
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
