using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GtrTraingHr.Models
{

    public class Attendance
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [DisplayName("Date")]
        public DateTime dtDate { get; set; }= DateTime.Now;
        [Required]
        [StringLength(50)]
        public string? AttStatus { get; set;}
        [Required]
        public DateTime InTime { get; set; }
        [Required]
        public DateTime OutTime { get; set; }
        [Required]
        [DisplayName("Company")]
        public string? CompanyId { get; set; }
        public Company? Company { get; set; }
        [Required]
        [DisplayName("Employee")]
        public string? EmpId { get; set; }
        public Employee? Emp { get; set; }

    }
    public class AttendanceDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
       
        public DateTime dtDate { get; set; } = DateTime.Now;
       
        public string? AttStatus { get; set; }
        
        public DateTime InTime { get; set; }
        
        public DateTime OutTime { get; set; }
        
        public string? CompanyId { get; set; }  
        public string? EmpId { get; set; }       
    }
}
