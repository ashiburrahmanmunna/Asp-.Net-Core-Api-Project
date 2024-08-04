using GtrTraingHr.Models;
using GtrTraingHr.Models.Const;
using GtrTraingHr.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace GtrTraingHr.ConsumeAPI.Controllers
{
    public class DesignationController : Controller
    {

       
        private readonly IDesignationService designationService;

        public DesignationController(IDesignationService designationService)
        {

            
            this.designationService = designationService;
        }
        // GET: DesignationController
        public async Task<ActionResult> Index()
        {
            var companylist = await designationService.GetAll();
            return View(companylist);
        }

        // GET: DesignationController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var singlecompany = await designationService.GetById(id);
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
                var singlecompany = await designationService.GetById(id);
                if (singlecompany == null)
                    return NotFound();
                return View(singlecompany);
            }


        }

        // POST: DesignationController/Create
        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(Designation model)
        {
            model.CompanyId = Request.Cookies[ApplicationConst.CompanyHeaderName].ToString();
            try
            {
                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString();
                  
                    await designationService.Create(model);
                }
                else
                {
                
                    await designationService.Update(model, model.Id);
                }


                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
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
               
                await designationService.Delete(id);
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
