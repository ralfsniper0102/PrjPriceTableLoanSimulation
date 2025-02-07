using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PrjPriceTableLoanSimulation.Application.Shared.Behavior;
using PrjPriceTableLoanSimulation.UseCase.UseCases.Simulate;

namespace PrjPriceTableLoanSimulation.Application.Services
{
    public static class ServiceExtensions
    {
        public static void ConfigureApplicantionApp(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<SimulateRequest>();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}
