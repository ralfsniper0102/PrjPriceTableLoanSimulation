using FluentValidation;

namespace PrjPriceTableLoanSimulation.UseCase.UseCases.GetSimulateById
{
    public class GetSimulateByIdValidation : AbstractValidator<GetSimulateByIdRequest>
    {
        public GetSimulateByIdValidation()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
