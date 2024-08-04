using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GtrTraingHr.Models
{
    
    public class Shift
    {
        [StringLength(40)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(50)]
        public string? ShiftName { get; set; }
        [Required]
        public DateTime ShiftIn { get; set; }
        [Required]
        public DateTime ShiftOut { get; set; }
        [Required]
        public DateTime ShiftLate { get; set; }

        [Required]
        public string? CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
