﻿using CompanyEmployees.DataAccess.Data;
using CompanyEmployees.DataAccess.GenericRepository.Service;
using CompanyEmployees.Domain.Models;
using CompanyEmployees.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Service.Services
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository 
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext) 
        { 
        }
    }
}
