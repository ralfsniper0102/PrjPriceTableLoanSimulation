using MediatR;
using Microsoft.EntityFrameworkCore;
using PrjPriceTableLoanSimulation.Domain.Entities;
using PrjPriceTableLoanSimulation.UseCase.Interfaces;

namespace PrjPriceTableLoanSimulation.UseCase.UseCases.GetSimulates
{
    public class GetSimulatesRequestHandler : IRequestHandler<GetSimulatesRequest, GetSimulatesResponse>
    {
        private readonly IGenericRepository<Proposal, int> _proposalRepository;

        public GetSimulatesRequestHandler(
            IGenericRepository<Proposal, int> proposalRepository)
        {
            _proposalRepository = proposalRepository;
        }

        public async Task<GetSimulatesResponse> Handle(GetSimulatesRequest request, CancellationToken cancellationToken)
        {
            int page = request.Page;
            int pageSize = request.PageSize;

            int skip = Math.Max(page - 1, 0) * pageSize;

            var query = await _proposalRepository.Queryable(x => x.Id > 0)
                                                    .Include(x => x.PaymentSchedules)
                                                    .OrderByDescending(x => x.Id)
                                                    .AsNoTracking()
                                                    .ToListAsync(cancellationToken: cancellationToken);

            var qt = query.Count();
            var qtPages = (int)Math.Ceiling((double)qt / pageSize);

            query = query
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            List<SimulateViewModel> simulates = new List<SimulateViewModel>();

            foreach (var item in query)
            {
                var simulateViewModel = new SimulateViewModel
                {
                    LoanAmount = item.LoanAmount,
                    AnnualInterestRate = item.AnnualInterestRate,
                    NumberOfMonths = item.NumberOfMonths,
                    MonthlyPayment = item.MonthlyPayment,
                    TotalInterest = item.TotalInterest,
                    TotalPayment = item.TotalPayment,
                    PaymentSchedules = item.PaymentSchedules.Select(ps => new PaymentScheduleViewModel
                    {
                        Month = ps.Month,
                        Principal = ps.Principal,
                        Interest = ps.Interest,
                        Balance = ps.Balance
                    }).ToList()
                };
                simulates.Add(simulateViewModel);
            }

            GetSimulatesResponse getSimulatesResponse = new GetSimulatesResponse
            {
                Simulates = simulates,
                QtTotal = qt,
                QtPages = qtPages,
                Page = page,
                PageSize = pageSize
            };

            return getSimulatesResponse;
        }
    }
}
