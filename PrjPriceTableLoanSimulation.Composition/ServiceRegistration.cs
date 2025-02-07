using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrjPriceTableLoanSimulation.Infrastructure.Context;
using PrjPriceTableLoanSimulation.Infrastructure.Repositories;
using PrjPriceTableLoanSimulation.UseCase.Interfaces;

namespace PrjPriceTableLoanSimulation.Composition
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnStr");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        }
    }
}
