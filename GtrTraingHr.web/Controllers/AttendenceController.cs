using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GtrTraingHr.web.Controllers
{
    public class AttendenceController : Controller
    {
        private readonly IAttendanceRepo attendanceRepo;
        private readonly ISalaryRepo salaryRepo;
        private readonly IEmployeeRepo employeeRepo;
        private readonly ICompanyRepo companyRepo;
        private readonly IShiftRepo shiftRepo;

        public AttendenceController(IAttendanceRepo attendanceRepo,
            ISalaryRepo salaryRepo,
            IEmployeeRepo employeeRepo,ICompanyRepo companyRepo,IShiftRepo shiftRepo)
        {
            this.attendanceRepo = attendanceRepo;
            this.salaryRepo = salaryRepo;
            this.employeeRepo = employeeRepo;
            this.companyRepo = companyRepo;
            this.shiftRepo = shiftRepo;
        }
        public async Task<ActionResult> SalarySummary(DateTime? dateTime)
        {
            if (dateTime is null)
            {
                return View(new List<Salary>());
            }
            var month = dateTime?.Month;
            var year = dateTime?.Year;
            
            var salary = await salaryRepo
                .Where(x => x.CompanyId == Request.Cookies["CompanyID"] && x.dtMonth == month && x.dtYear == year)
                .Include(x=>x.Emp)
                .ToListAsync();

            return View(salary);
        }
        public async Task<ActionResult> SalaryPaid(string empId, int dtMonth, int dtYear)
        {

           
            var salary = await salaryRepo.Where(x => x.EmpId == empId && x.dtMonth == dtMonth && x.dtYear == dtYear).SingleOrDefaultAsync();
            if (salary == null)
                return NotFound();
            salary.IsPaid = true;
            salary.PaidAmount = salary.PayableAmount;
            await salaryRepo.Update(salary, salary.Id);

            return RedirectToAction(nameof(SalarySummary), new { dateTime = new DateTime(dtYear, dtMonth, 1) });



        }
        public async Task<ActionResult> AttendenceSummery(DateTime? dateTime)
        {
            if (dateTime is null)
            {
                return View(new List<AttendanceSummary>());
            }
            var month = dateTime?.Month;
            var year=dateTime?.Year;
            using (SqlConnection connection = new SqlConnection("Server=.;Database=GTHRDBW001;Trusted_Connection=True;MultipleActiveResultSets=true;encrypt=false"))
            {
                using (SqlCommand command = new SqlCommand($"EXEC ProcessAttenceSummery '{Request.Cookies["CompanyID"]}',{month},{year}", connection))
                {
                    try
                    {
                       
                        connection.Open();

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

            var attenden =await attendanceRepo.AttendanceSummaries(Request.Cookies["CompanyID"], month, year);
            return View(attenden);
        }
        // GET: AttendenceController
        public async Task<ActionResult> Index()
        {
            var companylist = await attendanceRepo.Where(x => x.CompanyId == Request.Cookies["CompanyID"]).Include(x=>x.Company).Include(x=>x.Emp).ToListAsync();
            //var companylist = await db.Attendances.Include(x=>x.Company).Include(x=>x.Emp)
            //    .Where(x => x.CompanyId == Request.Cookies["CompanyID"]).ToListAsync();
            return View(companylist);
        }

        // GET: AttendenceController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var singlecompany = await attendanceRepo.Single(x=>x.Id==id);
            if (singlecompany == null)
                return NotFound();

            return View(singlecompany);
        }

        // GET: AttendenceController/Create
        public async Task<ActionResult> CreateOrEdit(string id)
        {
            ViewBag.EmpId = await employeeRepo.DropDownData(x=>x.CompanyId== Request.Cookies["CompanyID"], "Id", "EmpName");
          
            if (string.IsNullOrWhiteSpace(id))
            {
                return View(new Attendance()
                {
                    Id = string.Empty
                });
            }
            else
            {
                var singlecompany = await attendanceRepo.Single(x => x.Id == id);
                if (singlecompany == null)
                    return NotFound();
                return View(singlecompany);
            }


        }

        // POST: AttendenceController/Create
        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(Attendance model)
        {
            ViewBag.EmpId = await employeeRepo.DropDownData(x => x.CompanyId == Request.Cookies["CompanyID"], "Id", "EmpName");
            var salary = await companyRepo.Single(x => x.Id == Request.Cookies["CompanyID"]);
            if (salary == null)
            {
               return RedirectToAction("Index", "Home");
            }
            var employeeshift=await employeeRepo.Where(x=>x.Id==model.EmpId).Select(x=>x.ShiftId).SingleOrDefaultAsync();
            var shift = await shiftRepo.Single(x => x.Id == employeeshift);
            try
            {
                if (model.InTime.TimeOfDay>shift.ShiftLate.TimeOfDay)
                {
                    model.AttStatus = "L";
                }
                if (model.InTime.TimeOfDay<=shift.ShiftIn.TimeOfDay)
                {
                    model.AttStatus = "P";
                }
                if (model.dtDate.DayOfWeek==DayOfWeek.Friday)
                {
                    model.AttStatus = "W";
                }
                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString();
                    model.CompanyId = Request.Cookies["CompanyID"];
                    
                    await attendanceRepo.Create(model);
                }
                else
                {
                    var olddata = await attendanceRepo.Single(x => x.Id == model.Id);
                    if (olddata == null)
                        return NotFound();
                    olddata.InTime = model.InTime;
                    olddata.OutTime = model.OutTime;
                    olddata.dtDate = model.dtDate;
                    olddata.AttStatus = model.AttStatus;
                    olddata.CompanyId = Request.Cookies["CompanyID"];
                    olddata.EmpId = model.EmpId;

                    await attendanceRepo.Update(olddata,model.Id);                 
                    


                }
               

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
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
                var olddata = await attendanceRepo.Single(x => x.Id == id);
                if (olddata == null)
                    return NotFound();

                await attendanceRepo.Delete(id);               
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
