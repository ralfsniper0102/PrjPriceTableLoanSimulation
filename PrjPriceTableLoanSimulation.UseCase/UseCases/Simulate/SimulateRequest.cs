using MediatR;

namespace PrjPriceTableLoanSimulation.UseCase.UseCases.Simulate
{
    public class SimulateRequest : IRequest<SimulateResponse>
    {
        public decimal LoanAmount { get; set; }
        public decimal AnnualInterestRate { get; set; }
        public int NumberOfMonths { get; set; }
    }
}
