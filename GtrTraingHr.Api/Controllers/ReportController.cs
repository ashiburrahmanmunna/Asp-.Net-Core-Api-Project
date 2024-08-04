using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;
using GtrTraingHr.Models.Const;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GtrTraingHr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ISalaryRepo salaryRepo;
        private readonly IDepartmentRepo departmentRepo;

        public ReportController(ISalaryRepo salaryRepo,IDepartmentRepo departmentRepo)
        {
            this.salaryRepo = salaryRepo;
            this.departmentRepo = departmentRepo;
        }
        //public async Task<IActionResult> SalarySummery()
        //{
        //    //ViewBag.Dept = await departmentRepo.DropDownData(x => x.CompanyId == Request.Cookies["CompanyID"], "Id", "DeptName");
        //    return Ok();
        //}
        [HttpGet("SalarySummeryreport")]
        public async Task<IActionResult> SalarySummeryreport(string dept,string month,string year)
        {
            
            using (SqlConnection connection = new SqlConnection("Server=.;Database=GTHRDBW001;Trusted_Connection=True;MultipleActiveResultSets=true;encrypt=false"))
            {
                using (SqlCommand command = new SqlCommand($"EXEC PrcSalarySummary @comId='{Request.Headers[ApplicationConst.CompanyHeaderName].ToString()}',@year={year},@month={month},@dept='{dept}'", connection))
                {
                    try
                    {

                        connection.Open();

                       var reader=await command.ExecuteReaderAsync();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        if (dt.Rows.Count>0)
                        {
                            var result = dt.AsEnumerable()
                            .Select(row => new SalaryReportViewModel
                            {
                                DeptName = row.Field<string>("DeptName"),
                                Month = row.Field<int>("dtMonth"),
                                Year = row.Field<int>("dtYear"),
                                Total_PaidAmount = row.Field<double>("Total_PaidAmount"),
                                Total_PayableAmount = row.Field<double>("Total_PayableAmount")

                            })
                            .ToList();
                            return Ok(result);
                        }

                        return Ok(new List<SalaryReportViewModel>());


                    }
                    catch (Exception ex)
                    {
                        
                        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                    }
                }
            }

            
        }
    }
}
