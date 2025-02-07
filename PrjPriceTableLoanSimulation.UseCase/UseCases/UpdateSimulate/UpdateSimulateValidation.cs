using FluentValidation;

namespace PrjPriceTableLoanSimulation.UseCase.UseCases.UpdateSimulate
{
    public class UpdateSimulateValidation : AbstractValidator<UpdateSimulateRequest>
    {
        public UpdateSimulateValidation()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.LoanAmount)
                .NotEmpty();

            RuleFor(x => x.AnnualInterestRate)
                .NotEmpty();

            RuleFor(x => x.NumberOfMonths)
                .NotEmpty();

            RuleFor(x => x.MonthlyPayment)
                .NotEmpty();

            RuleFor(x => x.TotalInterest)
                .NotEmpty();

            RuleFor(x => x.TotalPayment)
                .NotEmpty();
        }
    }
}
