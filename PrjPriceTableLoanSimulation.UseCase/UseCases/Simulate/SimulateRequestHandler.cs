using MediatR;
using PrjPriceTableLoanSimulation.Domain.Entities;
using PrjPriceTableLoanSimulation.UseCase.Interfaces;

namespace PrjPriceTableLoanSimulation.UseCase.UseCases.Simulate
{
    public class SimulateRequestHandler : IRequestHandler<SimulateRequest, SimulateResponse>
    {
        private readonly IGenericRepository<Proposal, int> _proposalRepository;

        public SimulateRequestHandler(
            IGenericRepository<Proposal, int> proposalRepository)
        {
            _proposalRepository = proposalRepository;
        }

        public async Task<SimulateResponse> Handle(SimulateRequest request, CancellationToken cancellationToken)
        {
            decimal monthlyInterestRate = request.AnnualInterestRate / 12;
            double rateFactor = Math.Pow((double)(1 + monthlyInterestRate), request.NumberOfMonths);

            decimal pmt = request.LoanAmount * monthlyInterestRate * (decimal)rateFactor / ((decimal)rateFactor - 1);
            decimal remainingBalance = request.LoanAmount;
            decimal totalInterest = 0;

            List<int> months = Enumerable.Range(1, request.NumberOfMonths).ToList(); 
            List<PaymentSchedule> schedule = new();

            foreach (var month in months)
            {
                decimal interest = remainingBalance * monthlyInterestRate;
                decimal principal = pmt - interest;
                remainingBalance -= principal;
                totalInterest += interest;

                if (month == request.NumberOfMonths)
                    remainingBalance = 0;

                schedule.Add(new PaymentSchedule
                {
                    Month = month,
                    Principal = Math.Round(principal, 2),
                    Interest = Math.Round(interest, 2),
                    Balance = Math.Round(remainingBalance, 2)
                });
            }

            var proposal = new Proposal
            {
                LoanAmount = request.LoanAmount,
                AnnualInterestRate = request.AnnualInterestRate,
                NumberOfMonths = request.NumberOfMonths,
                MonthlyPayment = Math.Round(pmt, 2),
                TotalInterest = Math.Round(totalInterest, 2),
                TotalPayment = Math.Round(pmt * request.NumberOfMonths, 2),
                PaymentSchedules = schedule.Select(ps => new Domain.Entities.PaymentSchedule
                {
                    Month = ps.Month,
                    Principal = ps.Principal,
                    Interest = ps.Interest,
                    Balance = ps.Balance
                }).ToList()
            };
            
            var result = await _proposalRepository.InsertAsync(proposal);

            return new SimulateResponse
            {
                MontlyPayment = Math.Round(pmt, 2),
                TotalInterest = Math.Round(totalInterest, 2),
                TotalPayment = Math.Round(pmt * request.NumberOfMonths, 2),
                PaymentSchedule = schedule
            };
        }
    }
}
