using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GtrTraingHr.Models
{
  
    public class Department
    {
        [StringLength(40)]
        public string Id { get; set; }= Guid.NewGuid().ToString();
        [Required]
        [StringLength(150)]
        public string? DeptName { get; set; }
        [Required]
        public string? CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
