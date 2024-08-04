namespace GtrTraingHr.Models
{
    public class SalaryReportViewModel
    {
        public string DeptName { get; set; }=string.Empty;
        public int Month { get; set; } 
        public int Year { get; set; } 
        public double Total_PayableAmount { get; set; } 
        public double Total_PaidAmount { get; set; } 
    }
}
