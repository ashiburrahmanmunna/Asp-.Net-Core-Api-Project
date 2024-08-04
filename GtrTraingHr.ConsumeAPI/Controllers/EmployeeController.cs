
using GtrTraingHr.Models;
using GtrTraingHr.Models.Const;
using GtrTraingHr.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GtrTraingHr.ConsumeAPI.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly IEmployeeService empService;
        private readonly IDesignationService designationService;
        private readonly IDepartmentService departmentService;
        private readonly IShiftService shiftService;
        private readonly ICompanyService companyService;

        public EmployeeController(IEmployeeService empService,
            IDesignationService designationService,
            IDepartmentService departmentService, IShiftService shiftService, ICompanyService companyService)
        {

            this.empService = empService;
            this.designationService = designationService;
            this.departmentService = departmentService;
            this.shiftService = shiftService;
            this.companyService = companyService;
        }

       // GET: EmployeeController
       [HttpPost]
        public async Task<ActionResult> EmployeeReport(string dept, string desig)
        {
            try
            {
                var emplist =await empService.EmployeeReport(dept, desig);
                if (emplist.Count > 0)
                {
                    return Json(emplist);
                }
               return Json(new List<EmpreportViewModel>());
            }
            catch (Exception e)
            {

                throw;
            }

        }
        public async Task<ActionResult> EmployeeReport()
        {
            ViewBag.Desig = await designationService.DropDownData();
            ViewBag.Dept = await departmentService.DropDownData();
            return View();
        }
        public async Task<ActionResult> Index()
        {
            var companylist = await empService.GetAll();
            return View(companylist);
        }

        // GET: EmployeeController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var singlecompany = await empService.GetById(id);
            if (singlecompany == null)
                return NotFound();

            return View(singlecompany);
        }

        // GET: EmployeeController/Create
        public async Task<ActionResult> CreateOrEdit(string id)
        {
            ViewBag.Desig = await designationService.DropDownData();
            ViewBag.Dept = await departmentService.DropDownData();
            ViewBag.shift = await shiftService.DropDownData();
            if (string.IsNullOrWhiteSpace(id))
            {
                return View(new Employee()
                {
                    Id = string.Empty
                });
            }
            else
            {
                var singlecompany = await empService.GetById(id);
                if (singlecompany == null)
                    return NotFound();
                return View(singlecompany);
            }


        }

        // POST: EmployeeController/Create
        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(Employee model)
        {
          
            var salary = Request.Cookies[ApplicationConst.CompanyHeaderName].ToString();
           
            if (salary == null)
            {
                return RedirectToAction("Index", "Home");
            }
            model.CompanyId = Request.Cookies[ApplicationConst.CompanyHeaderName].ToString();
            try
            {
                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString();
                    
                    
                    await empService.Create(model);
                }
                else
                {
                      
                    await empService.Update(model, model.Id);

                }


                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Desig = await designationService.DropDownData();
                ViewBag.Dept = await departmentService.DropDownData();
                ViewBag.shift = await shiftService.DropDownData();
                ModelState.AddModelError(string.Empty, e.Message);
                return View(model);
            }
        }
        // POST: EmployeeController/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var olddata = await empService.GetById(id);
                if (olddata == null)
                    return NotFound();

                await empService.Delete(id);

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
