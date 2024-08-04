using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;
using GtrTraingHr.Models.Const;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GtrTraingHr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceRepo attendanceRepo;
        private readonly ISalaryRepo salaryRepo;
        private readonly IEmployeeRepo employeeRepo;
        private readonly ICompanyRepo companyRepo;
        private readonly IShiftRepo shiftRepo;

        public AttendanceController(IAttendanceRepo attendanceRepo,
            ISalaryRepo salaryRepo,
            IEmployeeRepo employeeRepo,ICompanyRepo companyRepo,IShiftRepo shiftRepo)
        {
            this.attendanceRepo = attendanceRepo;
            this.salaryRepo = salaryRepo;
            this.employeeRepo = employeeRepo;
            this.companyRepo = companyRepo;
            this.shiftRepo = shiftRepo;
        }
       
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                var companylist = await attendanceRepo.Where(x => x.CompanyId == Request.Headers[ApplicationConst.CompanyHeaderName].ToString()).Include(x => x.Company).Include(x => x.Emp).ToListAsync();
                //var companylist = await db.Attendances.Include(x=>x.Company).Include(x=>x.Emp)
                //    .Where(x => x.CompanyId == Request.Cookies["CompanyID"]).ToListAsync();
                return Ok(companylist);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); 
            }
            
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            try
            {
                var singlecompany = await attendanceRepo.Single(x => x.Id == id);
                if (singlecompany == null)
                    return NotFound();

                return Ok(singlecompany);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
           
        }

       
        [HttpPost]
        public async Task<ActionResult> Create(AttendanceDTO model)
        {
            try
            {
                // Fetch the salary and handle redirection if it doesn't exist
                var salary = await companyRepo.Single(x => x.Id == Request.Headers[ApplicationConst.CompanyHeaderName].ToString());               

                // Fetch the employee's shift and determine the attendance status based on the provided conditions
                var employeeshift = await employeeRepo.Where(x => x.Id == model.EmpId).Select(x => x.ShiftId).SingleOrDefaultAsync();
                var shift = await shiftRepo.Single(x => x.Id == employeeshift);
                if (model.InTime.TimeOfDay > shift.ShiftLate.TimeOfDay)
                {
                    model.AttStatus = "L";
                }
                else if (model.InTime.TimeOfDay <= shift.ShiftIn.TimeOfDay)
                {
                    model.AttStatus = "P";
                }
                else if (model.dtDate.DayOfWeek == DayOfWeek.Friday)
                {
                    model.AttStatus = "W";
                }

                model.Id = Guid.NewGuid().ToString();
                
                var attendan = new Attendance
                {
                    CompanyId = model.CompanyId,
                    AttStatus = model.AttStatus,
                    EmpId = model.EmpId,
                    InTime = model.InTime,
                    OutTime = model.OutTime,
                    dtDate = model.dtDate,

                };
                await attendanceRepo.Create(attendan);

                return Ok();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return Ok(model);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(string id,AttendanceDTO model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest("Id Mismatch");
                }
                var olddata = await attendanceRepo.Single(x => x.Id == model.Id);
                if (olddata == null)
                    return NotFound();

                // Fetch the salary and handle redirection if it doesn't exist
                var salary = await companyRepo.Single(x => x.Id == Request.Headers[ApplicationConst.CompanyHeaderName].ToString());
                if (salary == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                // Fetch the employee's shift and determine the attendance status based on the provided conditions
                var employeeshift = await employeeRepo.Where(x => x.Id == model.EmpId).Select(x => x.ShiftId).SingleOrDefaultAsync();
                var shift = await shiftRepo.Single(x => x.Id == employeeshift);
                if (model.InTime.TimeOfDay > shift.ShiftLate.TimeOfDay)
                {
                    model.AttStatus = "L";
                }
                else if (model.InTime.TimeOfDay <= shift.ShiftIn.TimeOfDay)
                {
                    model.AttStatus = "P";
                }
                else if (model.dtDate.DayOfWeek == DayOfWeek.Friday)
                {
                    model.AttStatus = "W";
                }

                olddata.InTime = model.InTime;
                olddata.OutTime = model.OutTime;
                olddata.dtDate = model.dtDate;
                olddata.AttStatus = model.AttStatus;
                olddata.CompanyId = model.CompanyId;
                olddata.EmpId = model.EmpId;

                await attendanceRepo.Update(olddata, model.Id);

                return Ok();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return Ok(model);
            }
        }

        // POST: AttendenceController/Delete/5
        [HttpDelete("{id}")]
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
                return StatusCode(StatusCodes.Status500InternalServerError,"Error while deleting");
            }
        }
        [HttpGet("SalarySummary")]
        public async Task<ActionResult> SalarySummary(DateTime? dateTime)
        {
            try
            {
                if (dateTime is null)
                {
                    return Ok(new List<Salary>());
                }
                var month = dateTime?.Month;
                var year = dateTime?.Year;

                var salary = await salaryRepo
                    .Where(x => x.CompanyId == Request.Headers[ApplicationConst.CompanyHeaderName].ToString() && x.dtMonth == month && x.dtYear == year)
                    .Include(x => x.Emp)
                    .ToListAsync();

                return Ok(salary);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [HttpGet("SalaryPaid")]
        public async Task<ActionResult> SalaryPaid(string empId, int dtMonth, int dtYear)
        {
            try
            {
                var salary = await salaryRepo.Where(x => x.EmpId == empId && x.dtMonth == dtMonth && x.dtYear == dtYear).SingleOrDefaultAsync();
                if (salary == null)
                    return NotFound();
                salary.IsPaid = true;
                salary.PaidAmount = salary.PayableAmount;
                await salaryRepo.Update(salary, salary.Id);

                return Ok("Update SuccessFully");
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }





        }
        [HttpGet("AttendenceSummery")]
        public async Task<ActionResult> AttendenceSummery(DateTime? dateTime)
        {
            try
            {
                if (dateTime is null)
                {
                    return Ok(new List<AttendanceSummary>());
                }
                var month = dateTime?.Month;
                var year = dateTime?.Year;
                using (SqlConnection connection = new SqlConnection("Server=.;Database=GTHRDBW001;Trusted_Connection=True;MultipleActiveResultSets=true;encrypt=false"))
                {
                    using (SqlCommand command = new SqlCommand($"EXEC ProcessAttenceSummery '{Request.Headers[ApplicationConst.CompanyHeaderName].ToString()}',{month},{year}", connection))
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

                var attenden = await attendanceRepo.AttendanceSummaries(Request.Headers[ApplicationConst.CompanyHeaderName].ToString(), month, year);
                return Ok(attenden);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}
