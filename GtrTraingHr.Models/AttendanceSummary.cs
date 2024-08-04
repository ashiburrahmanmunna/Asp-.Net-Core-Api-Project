using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GtrTraingHr.Models
{
    
    public class AttendanceSummary
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [DisplayName("Year")]
        public int dtYear { get; set; }
        [Required]
        [DisplayName("Month")]
        public int dtMonth { get; set; }
        [Required]
        public string? CompanyId { get; set; }
        public Company? Company { get; set; }

        [Required]
        [DisplayName("Employee")]
        public string? EmpId { get; set; }
        public Employee? Emp { get; set; }
        public int Present{ get; set; }
        public int Absent{ get; set; }
        public int Late{ get; set; }
        public int Weekend { get; set; }

  
    }
}
