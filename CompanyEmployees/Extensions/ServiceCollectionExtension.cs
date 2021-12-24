using AutoMapper;
using CompanyEmployees.DataAccess.Data;
using CompanyEmployees.Domain.Mappings;
using CompanyEmployees.LoggerService.Interfaces;
using CompanyEmployees.LoggerService.Services;
using CompanyEmployees.Service.Interfaces;
using CompanyEmployees.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


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
            var mapperConfig = new MapperConfiguration(map =>
            {
                map.AddProfile<CompanyMappingProfile>();
            });
            services.AddSingleton(mapperConfig.CreateMapper());
        }
    }
}
