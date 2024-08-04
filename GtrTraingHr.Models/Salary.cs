using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GtrTraingHr.Models
{
    
    public class Salary
    {
        [StringLength(40)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public int dtYear { get; set; }
        [Required]
        public int dtMonth { get; set; }
        public double Gross { get; set; }
       
        public double Basic { get; set; }
        
        public double HRent { get; set; }
        
        public double Medical { get; set; }
        public double AbsentAmount { get; set; }
        public double PayableAmount { get; set; }
        public bool IsPaid { get; set; } = false;
        public double PaidAmount { get; set;}
        [Required]
        public string? CompanyId { get; set; }
        public Company? Company { get; set; }

        [Required]
        public string? EmpId { get; set; }
        public Employee? Emp { get; set; }
        

    }
}
