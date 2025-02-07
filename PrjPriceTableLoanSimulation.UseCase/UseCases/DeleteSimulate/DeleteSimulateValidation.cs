using FluentValidation;

namespace PrjPriceTableLoanSimulation.UseCase.UseCases.DeleteSimulate
{
    public class DeleteSimulateValidation : AbstractValidator<DeleteSimulateRequest>
    {
        public DeleteSimulateValidation()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
