using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GtrTraingHr.Models
{
   
    public class Employee
    {
        [StringLength(40)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(150)]
        [DisplayName("Employee Name")]
        public string? EmpName { get; set; }
        [Required]        
        public Gender Gender { get; set; }
        //public string? Gender { get; set; }
        [Required]
        [Range (10000, 100000)]
        public double Gross { get; set; }
        [Required]
        public double Basic { get; set; }
        [Required]
        [DisplayName("House Rent")]
        public double HRent { get; set; }
        [Required]
        public double Medical { get; set; }
        [Required]
        public double Others { get; set; }
        [Required]
        [DisplayName("Joining Date")]
        public DateTime dtJoin { get; set; }= DateTime.Now;
        [Required]
        [DisplayName("Company")]
        public string? CompanyId { get; set; }
        public Company? Company { get; set; }

        [Required]
        [DisplayName("Shift")]
        public string? ShiftId { get; set; }
        public Shift? Shift { get; set; }
        [Required]
        [DisplayName("Department")]
        public string? DeptId { get; set;}
        public Department? Dept { get; set; }
        [Required]
        [DisplayName("Desiganation")]
        public string? DesigId { get; set; }
        public Designation? Desig { get; set; }
        


    }
    public enum Gender
    {
        Male,
        Female,
        Other
    }

}
