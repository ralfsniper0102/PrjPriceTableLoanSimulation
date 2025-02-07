using FluentValidation;

namespace PrjPriceTableLoanSimulation.UseCase.UseCases.Simulate
{
    public class SimulateValitation : AbstractValidator<SimulateRequest>
    {
        public SimulateValitation()
        {
            RuleFor(x => x.LoanAmount)
                .GreaterThan(0);

            RuleFor(x => x.AnnualInterestRate)
                .GreaterThan(0)
                .LessThanOrEqualTo(1);

            RuleFor(x => x.NumberOfMonths)
                .GreaterThan(0);
        }
    }
}
