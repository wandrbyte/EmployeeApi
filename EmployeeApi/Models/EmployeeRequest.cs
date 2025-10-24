using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.Models
{
    public class EmployeeRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateOnly DateOfBirth { get; set; }
    }
}
