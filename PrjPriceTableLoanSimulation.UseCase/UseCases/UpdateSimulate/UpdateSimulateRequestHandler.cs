using MediatR;
using Microsoft.EntityFrameworkCore;
using PrjPriceTableLoanSimulation.Domain.Entities;
using PrjPriceTableLoanSimulation.Exception.Exceptions;
using PrjPriceTableLoanSimulation.Localization.Localizations;
using PrjPriceTableLoanSimulation.UseCase.Interfaces;

namespace PrjPriceTableLoanSimulation.UseCase.UseCases.UpdateSimulate
{
    public class UpdateSimulateRequestHandler : IRequestHandler<UpdateSimulateRequest, Unit>
    {
        private readonly IGenericRepository<Proposal, int> _proposalRepository;
        private readonly Resources _resources;

        public UpdateSimulateRequestHandler(
            IGenericRepository<Proposal, int> proposalRepository,
            Resources resources)
        {
            _proposalRepository = proposalRepository;
            _resources = resources;
        }

        public async Task<Unit> Handle(UpdateSimulateRequest request, CancellationToken cancellationToken)
        {
            var result = await _proposalRepository.Queryable(x => x.Id == request.Id).Include(x => x.PaymentSchedules).FirstOrDefaultAsync();

            if (result == null)
                throw new ConflictException(_resources.ProposalNotExists());

            result.LoanAmount = request.LoanAmount;
            result.AnnualInterestRate = request.AnnualInterestRate;
            result.NumberOfMonths = request.NumberOfMonths;
            result.MonthlyPayment = request.MonthlyPayment;
            result.TotalInterest = request.TotalInterest;
            result.TotalPayment = request.TotalPayment;
            result.PaymentSchedules = new List<PaymentSchedule>();

            foreach (var schedule in request.PaymentSchedules)
            {
                var paymentSchedule = new PaymentSchedule
                {
                    Month = schedule.Month, 
                    Principal = schedule.Principal,
                    Interest = schedule.Interest,
                    Balance = schedule.Balance
                };

                result.PaymentSchedules.Add(paymentSchedule);
            }

            await _proposalRepository.UpdateAsync(result);

            return Unit.Value;
        }
    }
}
