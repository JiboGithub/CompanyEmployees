using AutoMapper;
using CompanyEmployees.DataAccess.Data;
using CompanyEmployees.Domain.Mappings;
using CompanyEmployees.LoggerService.Interfaces;
using CompanyEmployees.LoggerService.Services;
using CompanyEmployees.Service.Filters;
using CompanyEmployees.Service.Filters.ActionFilters;
using CompanyEmployees.Service.Interfaces;
using CompanyEmployees.Service.ResponseFormatters;
using CompanyEmployees.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace CompanyEmployees.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(
                options => { options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                });

        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddScoped<ILoggerManager, LoggerManager>();

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<RepositoryContext>(
                opts => opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                    b => b.MigrationsAssembly("CompanyEmployees.DataAccess")));

        public static void ConfigureRepositoryManager(this IServiceCollection services)
            => services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureMapping(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            var mapperConfig = new MapperConfiguration(map =>
            {
                map.AddProfile<CompanyMappingProfile>();
                map.AddProfile<EmployeeMappingProfile>();
            });
            services.AddSingleton(mapperConfig.CreateMapper());
        }

        public static void ConfigureControllers(this IServiceCollection services)
            => services.AddControllers(config =>
            {
                //config.Filters.Add(new ActionFilter());
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters()
            .AddCustomCSVFormatter();

        public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) 
            => builder.AddMvcOptions(
                config => config.OutputFormatters.Add(new CsvOutputFormatter()));

        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>();
            services.AddScoped<ValidateCompanyExistsAttribute>();
            services.AddScoped<ValidateEmployeeForCompanyExistsAttribute>();
        }
    }
}
