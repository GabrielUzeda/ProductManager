using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using ProductManager.Application.Common.Mappings;

namespace ProductManager.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            
            // Registrar o MediatR
            services.AddMediatR(cfg => 
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            
            // Registrar os perfis de mapeamento
            services.AddAutoMapper(cfg => 
            {
                cfg.AddProfile<MappingProfile>();
            });

            return services;
        }
    }
}
