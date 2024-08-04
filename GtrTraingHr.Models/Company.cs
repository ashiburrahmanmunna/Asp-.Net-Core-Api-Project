using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GtrTraingHr.Models
{
    public class Company
    {
        [StringLength(40)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(200)]
        [DisplayName("Company Name")]
        public string? ComName { get; set; }
        [Required]
        public double Basic { get; set; }
        [Required]
        [DisplayName("House Rent")]
        public double HRent { get; set; }
        [Required]
        public double Medical { get; set; }
        public bool IsInactive { get; set; } = true;
    }
}
