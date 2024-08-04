using GtrTraingHr.Models;
using GtrTraingHr.Services.IService;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GtrTraingHr.ConsumeAPI.Controllers
{
    public class ReportController : Controller
    {
        private readonly IAttendanceService attendanceService;
        private readonly IDepartmentService departmentService;

        public ReportController(IAttendanceService attendanceService, IDepartmentService departmentService)
        {
            this.attendanceService = attendanceService;
            this.departmentService = departmentService;
        }
        public async Task<IActionResult> SalarySummery()
        {
            ViewBag.Dept = await departmentService.DropDownData();
            return View();
        }
        public async Task<IActionResult> SalarySummeryreport(string dept, string month, string year)
        {


            try
            {
                List<SalaryReportViewModel> salarySummery = await attendanceService.SalarySummeryreport(dept, month, year);

                if (salarySummery.Count > 0)
                {

                    return Json(salarySummery);
                }

                return Json(new List<SalaryReportViewModel>());


            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.Message);
                return Json(ex.Message);
            }
        }
    }




}
