using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Domain.Dtos
{
    public class CompanyDto 
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } 
        public string FullAddress { get; set; }
    }

    public class CompanyForCreationDto : CompanyForManipulationDto
    {
    }

    public class CompanyForUpdateDto : CompanyForManipulationDto
    {
    }


    public abstract class CompanyForManipulationDto
    {
        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Address is 30 characters.")]
        public string Address { get; set; }


        [Required(ErrorMessage = "Country is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for Country is 20 characters.")]
        public string Country { get; set; }

        public IEnumerable<EmployeeForCreationDto> Employees { get; set; }
    }
}
