using GtrTraingHr.Models;
using GtrTraingHr.Models.Const;
using GtrTraingHr.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace GtrTraingHr.ConsumeAPI.Controllers
{
    public class DepartmentController : Controller
    {

       
        private readonly IDepartmentService departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
           
            this.departmentService = departmentService;
        }
        // GET: DepartmentController
        public async Task<ActionResult> Index()
        {
            var companylist = await departmentService.GetAll();
            return View(companylist);
        }

        // GET: DepartmentController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var singlecompany = await departmentService.GetById(id);
            if (singlecompany == null)
                return NotFound();

            return View(singlecompany);
        }

        // GET: DepartmentController/Create
        public async Task<ActionResult> CreateOrEdit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return View(new Department()
                {
                    Id = string.Empty
                });
            }
            else
            {
                var singlecompany = await departmentService.GetById(id);
                if (singlecompany == null)
                    return NotFound();
                return View(singlecompany);
            }


        }

        // POST: DepartmentController/Create
        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(Department model)
        {
            model.CompanyId = Request.Cookies[ApplicationConst.CompanyHeaderName].ToString();
            try
            {
                if (string.IsNullOrWhiteSpace(model.Id)) 
                {
                    model.Id = Guid.NewGuid().ToString();
                    await departmentService.Create(model);
                }
                else
                {
                    await departmentService.Update(model, model.Id);

                }


                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(model);
            }
        }
        // POST: DepartmentController/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await departmentService.Delete(id);

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
