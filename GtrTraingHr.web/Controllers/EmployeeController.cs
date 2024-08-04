using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GtrTraingHr.web.Controllers
{
    public class EmployeeController : Controller
    {
     
        private readonly IEmployeeRepo empRepo;
        private readonly IDesignationRepo designationRepo;
        private readonly IDepartmentRepo departmentRepo;
        private readonly IShiftRepo shiftRepo;
        private readonly ICompanyRepo companyRepo;

        public EmployeeController(IEmployeeRepo empRepo,
            IDesignationRepo designationRepo,
            IDepartmentRepo departmentRepo,IShiftRepo shiftRepo,ICompanyRepo companyRepo)
        {
           
            this.empRepo = empRepo;
            this.designationRepo = designationRepo;
            this.departmentRepo = departmentRepo;
            this.shiftRepo = shiftRepo;
            this.companyRepo = companyRepo;
        }
        
        // GET: EmployeeController
        [HttpPost]
        public async Task<ActionResult> EmployeeReport(string dept,string desig)
        {
            try
            {
                var companylist = empRepo.Where(x => x.CompanyId == Request.Cookies["CompanyID"]);
                if (!string.IsNullOrEmpty(dept))
                {
                    companylist = companylist.Where(x => x.DeptId == dept);
                }
                if (!string.IsNullOrEmpty(desig))
                {
                    companylist = companylist.Where(x => x.DesigId == desig);
                }
                var emp = await companylist.Select(x => new EmpreportViewModel
                {
                  EmpName= x.EmpName,
                  ComName=  x.Company.ComName,
                   DeptName= x.Dept.DeptName,
                   Gross= x.Gross,
                  Basic=  x.Basic,
                   HRent= x.HRent,
                  Medical=  x.Medical,
                   Others= x.Others,
                    dtJoin = x.dtJoin.ToShortDateString(),
                    Gender= x.Gender,
                    ShiftName= x.Shift.ShiftName,
                    jd= x.dtJoin


                }).ToListAsync();
                return Json(emp);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public async Task<ActionResult> EmployeeReport()
        {
            ViewBag.Desig =await designationRepo.DropDownData(x => x.CompanyId == Request.Cookies["CompanyID"], "Id", "DesigName");
            ViewBag.Dept = await departmentRepo.DropDownData(x => x.CompanyId == Request.Cookies["CompanyID"], "Id", "DeptName");
            return View();
        }
        public async Task<ActionResult> Index()
        {
            var companylist = await empRepo.Where(x => x.CompanyId == Request.Cookies["CompanyID"]).ToListAsync();
            return View(companylist);
        }

        // GET: EmployeeController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var singlecompany = await empRepo.Single(x => x.Id == id);
            if (singlecompany == null)
                return NotFound();

            return View(singlecompany);
        }

        // GET: EmployeeController/Create
        public async Task<ActionResult> CreateOrEdit(string id)
        {
            ViewBag.Desig = await designationRepo.DropDownData(x => x.CompanyId == Request.Cookies["CompanyID"], "Id", "DesigName");
            ViewBag.Dept = await departmentRepo.DropDownData(x => x.CompanyId == Request.Cookies["CompanyID"], "Id", "DeptName");
            ViewBag.shift = await shiftRepo.DropDownData(x => x.CompanyId == Request.Cookies["CompanyID"], "Id", "ShiftName");
            if (string.IsNullOrWhiteSpace(id))
            {
                return View(new Employee()
                {
                    Id = string.Empty
                });
            }
            else
            {
                var singlecompany = await empRepo.Where(x => x.Id == id).SingleOrDefaultAsync();
                if (singlecompany == null)
                    return NotFound();
                return View(singlecompany);
            }


        }

        // POST: EmployeeController/Create
        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(Employee model)
        {
            ViewBag.Desig = await designationRepo.DropDownData(x => x.CompanyId == Request.Cookies["CompanyID"], "Id", "DesigName");
            ViewBag.Dept = await departmentRepo.DropDownData(x => x.CompanyId == Request.Cookies["CompanyID"], "Id", "DeptName");
            ViewBag.shift = await shiftRepo.DropDownData(x => x.CompanyId == Request.Cookies["CompanyID"], "Id", "ShiftName");
            var salary = await companyRepo.Single(x => x.Id == Request.Cookies["CompanyID"]);
            if (salary == null)
            {
               return RedirectToAction("Index", "Home");
            }
            try
            {
                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString();
                    model.CompanyId = Request.Cookies["CompanyID"];
                    model.Basic =model.Gross*(salary.Basic / 100);
                    model.HRent =model.Gross*(salary.HRent / 100);
                    model.Medical =model.Gross*(salary.Medical / 100);
                    model.Others = model.Gross - (model.Basic + model.HRent + model.Medical);
                    await empRepo.Create(model);
                }
                else
                {
                    var olddata = await empRepo.Where(x => x.Id == model.Id).SingleOrDefaultAsync();
                    if (olddata == null)
                        return NotFound();
                    olddata.EmpName = model.EmpName;
                    olddata.DeptId = model.DeptId;
                    olddata.DesigId = model.DesigId;
                    olddata.CompanyId = Request.Cookies["CompanyID"];
                    olddata.Gross = model.Gross;
                    olddata.dtJoin = model.dtJoin;
                    olddata.Basic = model.Gross * (salary.Basic / 100);
                    olddata.HRent = model.Gross * (salary.HRent / 100);
                    olddata.Medical = model.Gross * (salary.Medical / 100);
                    olddata.Others = model.Gross - (model.Basic + model.HRent + model.Medical);
                    olddata.ShiftId = model.ShiftId;

                    await empRepo.Update(olddata, model.Id);

                }
              

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
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
                var olddata = await empRepo.Where(x => x.Id == id).SingleOrDefaultAsync();
                if (olddata == null)
                    return NotFound();

               await empRepo.Delete(id);
               
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
