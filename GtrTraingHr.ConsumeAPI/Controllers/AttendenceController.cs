using GtrTraingHr.Models;
using GtrTraingHr.Models.Const;
using GtrTraingHr.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GtrTraingHr.ConsumeAPI.Controllers
{
    public class AttendenceController : Controller
    {
        private readonly IAttendanceService attendanceService;
        private readonly IEmployeeService employeeService;
        private readonly ICompanyService companyService;

        public AttendenceController(IAttendanceService attendanceService,
            IEmployeeService employeeService, ICompanyService companyService)
        {
            this.attendanceService = attendanceService;
            this.employeeService = employeeService;
            this.companyService = companyService;
        }
        public async Task<ActionResult> SalarySummary(DateTime? dateTime)
        {
            if (dateTime is null)
            {
                return View(new List<Salary>());
            }


            var salary = await attendanceService.SalarySummary(Convert.ToDateTime(dateTime));


            return View(salary);
        }
        public async Task<ActionResult> SalaryPaid(string empId, int dtMonth, int dtYear)
        {


            var responseMessage = await attendanceService.SalaryPaid(empId, dtMonth, dtYear);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(SalarySummary), new { dateTime = new DateTime(dtYear, dtMonth, 1) });
            }
            else
            {
                //alert  massage 
                return RedirectToAction(nameof(SalarySummary), new { dateTime = new DateTime(dtYear, dtMonth, 1) });
            }





        }
        public async Task<ActionResult> AttendenceSummery(DateTime? dateTime)
        {
            if (dateTime is null)
            {
                return View(new List<AttendanceSummary>());
            }
            var attenden = await attendanceService.AttendanceSummaries(Convert.ToDateTime(dateTime));
            return View(attenden);
        }
        // GET: AttendenceController
        public async Task<ActionResult> Index()
        {
            var companylist = await attendanceService.GetAll();
            //var companylist = await db.Attendances.Include(x=>x.Company).Include(x=>x.Emp)
            //    .Where(x => x.CompanyId == Request.Headers[ApplicationConst.CompanyHeaderName].ToString()).ToListAsync();
            return View(companylist);
        }

        // GET: AttendenceController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var singlecompany = await attendanceService.GetById(id);
            if (singlecompany == null)
                return NotFound();

            return View(singlecompany);
        }

        // GET: AttendenceController/Create
        public async Task<ActionResult> CreateOrEdit(string id)
        {
            ViewBag.EmpId = await employeeService.DropDownData();

            if (string.IsNullOrWhiteSpace(id))
            {
                return View(new AttendanceDTO()
                {
                    Id = string.Empty
                });
            }
            else
            {
                var singlecompany = await attendanceService.GetById(id);
                if (singlecompany == null)
                    return NotFound();
                var atten = new AttendanceDTO()
                {
                    CompanyId = singlecompany.CompanyId,
                    AttStatus=singlecompany.AttStatus,
                    dtDate=singlecompany.dtDate,
                    OutTime=singlecompany.OutTime,
                    InTime=singlecompany.InTime,
                    EmpId=singlecompany.EmpId,
                    Id=singlecompany.Id,
                };
               
                return View(atten);
            }


        }

        // POST: AttendenceController/Create
        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(AttendanceDTO model)
        {

            var company = await companyService.GetById(Request.Cookies[ApplicationConst.CompanyHeaderName].ToString());
            if (company == null)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {

                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id=Guid.NewGuid().ToString();
                    model.CompanyId = Request.Cookies[ApplicationConst.CompanyHeaderName].ToString();
                   
                    await attendanceService.Create(model);
                }
                else
                {
                    await attendanceService.Update(model, model.Id);

                }


                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.EmpId = await employeeService.DropDownData();
                ModelState.AddModelError(string.Empty, e.Message);
                return View(model);
            }
        }
        // POST: AttendenceController/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
               
                await attendanceService.Delete(id);
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
