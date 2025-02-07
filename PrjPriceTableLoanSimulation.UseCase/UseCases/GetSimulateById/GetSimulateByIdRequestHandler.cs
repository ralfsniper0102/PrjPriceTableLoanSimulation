using MediatR;
using Microsoft.EntityFrameworkCore;
using PrjPriceTableLoanSimulation.Domain.Entities;
using PrjPriceTableLoanSimulation.Exception.Exceptions;
using PrjPriceTableLoanSimulation.Localization.Localizations;
using PrjPriceTableLoanSimulation.UseCase.Interfaces;
using PrjPriceTableLoanSimulation.UseCase.UseCases.GetSimulates;

namespace PrjPriceTableLoanSimulation.UseCase.UseCases.GetSimulateById
{
    public class GetSimulateByIdRequestHandler : IRequestHandler<GetSimulateByIdRequest, GetSimulateByIdResponse>
    {
        private readonly IGenericRepository<Proposal, int> _proposalRepository;
        private readonly Resources _resources;

        public GetSimulateByIdRequestHandler(
            IGenericRepository<Proposal, int> proposalRepository,
            Resources resources)
        {
            _proposalRepository = proposalRepository;
            _resources = resources;
        }

        public async Task<GetSimulateByIdResponse> Handle(GetSimulateByIdRequest request, CancellationToken cancellationToken)
        {
            var result = await _proposalRepository.Queryable(x => x.Id == request.Id).Include(x => x.PaymentSchedules).FirstOrDefaultAsync();

            if (result == null)
                throw new ConflictException(_resources.ProposalNotExists());

            GetSimulateByIdResponse getSimulateByIdResponse = new GetSimulateByIdResponse
            {
                LoanAmount = result.LoanAmount,
                AnnualInterestRate = result.AnnualInterestRate,
                NumberOfMonths = result.NumberOfMonths,
                MonthlyPayment = result.MonthlyPayment,
                TotalInterest = result.TotalInterest,
                TotalPayment = result.TotalPayment,
                PaymentSchedules = result.PaymentSchedules.Select(ps => new PaymentScheduleViewModel
                {
                    Month = ps.Month,
                    Principal = ps.Principal,
                    Interest = ps.Interest,
                    Balance = ps.Balance
                }).ToList()
            };

            return getSimulateByIdResponse;
        }
    }
}
