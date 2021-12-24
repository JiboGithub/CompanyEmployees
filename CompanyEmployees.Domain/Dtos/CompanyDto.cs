using System;
using System.Collections.Generic;
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
}
