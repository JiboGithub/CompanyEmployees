using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Domain.Dtos
{
    public class EmployeeDto 
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } 
        public int Age { get; set; } 
        public string Position { get; set; } 
    }

    public class EmployeeForCreationDto
    {
        public string Name { get; set; }
        public int Age { get; set; } 
        public string Position { get; set; } 
    }

    public class EmployeeForUpdateDto 
    {
        public string Name { get; set; }
        public int Age { get; set; } 
        public string Position { get; set; } 
    }
}
