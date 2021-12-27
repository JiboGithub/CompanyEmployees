using AutoMapper;
using CompanyEmployees.Domain.Dtos;
using CompanyEmployees.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Domain.Mappings
{
    public class CompanyMappingProfile : Profile 
    {
        public CompanyMappingProfile() 
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(
                c => c.FullAddress, 
                opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));

            CreateMap<CompanyForCreationDto, Company>();
        }
    }
}
