namespace GtrTraingHr.Models
{
    public class EmpreportViewModel
    {
        public string EmpName { get; set; }
        public string ComName { get; set; }
        public string DeptName { get; set; }
        public double Gross { get; set; }
        public double Basic { get; set; }
        public double HRent { get; set; }
        public double Medical { get; set; }
        public double Others { get; set; }
        public string dtJoin { get; set; }
        public DateTime jd { get; set; }
        public string ServicePeriod  => ServicePriod();
        public Gender Gender { get; set; }
        public string ShiftName { get; set; }

        private string ServicePriod()
        {
            DateTime joinDate = jd;
            DateTime currentDate = DateTime.Now;

            TimeSpan difference = currentDate - joinDate;

            int totalYears = currentDate.Year - joinDate.Year;
            int totalMonths = currentDate.Month - joinDate.Month;
            int totalDays = currentDate.Day - joinDate.Day;

            if (totalDays < 0)
            {
                totalMonths--;
                totalDays += DateTime.DaysInMonth(joinDate.Year, joinDate.Month);
            }

            if (totalMonths < 0)
            {
                totalYears--;
                totalMonths += 12;
            }

            string serviceLength = $"{totalYears} years, {totalMonths} months, {totalDays} days";
            return serviceLength;
        }
    }
}
