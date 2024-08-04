using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GtrTraingHr.web.Controllers
{
    public class DepartmentController : Controller
    {
      
        private readonly IDepartmentRepo departmentRepo;

        public DepartmentController(IDepartmentRepo departmentRepo)
        {
           this.departmentRepo = departmentRepo;
        }
        // GET: DepartmentController
        public async Task<ActionResult> Index()
        {
            var companylist = await departmentRepo.Where(x=>x.CompanyId== Request.Cookies["CompanyID"]).Include(x => x.Company).ToListAsync();
            return View(companylist);
        }

        // GET: DepartmentController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var singlecompany= await departmentRepo.Single(x=>x.Id==id);
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
                var singlecompany = await departmentRepo.Single(x => x.Id == id);
                if (singlecompany == null)
                    return NotFound();
                return View(singlecompany);
            }

          
        }

        // POST: DepartmentController/Create
        [HttpPost]       
        public async Task<ActionResult> CreateOrEdit(Department model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id=Guid.NewGuid().ToString();
                    model.CompanyId = Request.Cookies["CompanyID"];
                    await departmentRepo.Create(model);
                }
                else
                {
                    var olddata = await departmentRepo.Single(x => x.Id == model.Id);
                    if (olddata == null)
                        return NotFound();
                    olddata.CompanyId = Request.Cookies["CompanyID"];
                    olddata.DeptName= model.DeptName;

                    await departmentRepo.Update(olddata, olddata.Id);

                }
                

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
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
                var olddata = await departmentRepo.Single(x => x.Id == id);
                if (olddata == null)
                    return NotFound();

               await departmentRepo.Delete(id);
             
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
