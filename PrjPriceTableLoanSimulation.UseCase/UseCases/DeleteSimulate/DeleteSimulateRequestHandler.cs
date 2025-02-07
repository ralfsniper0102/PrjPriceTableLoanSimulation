using MediatR;
using Microsoft.EntityFrameworkCore;
using PrjPriceTableLoanSimulation.Domain.Entities;
using PrjPriceTableLoanSimulation.Exception.Exceptions;
using PrjPriceTableLoanSimulation.Localization.Localizations;
using PrjPriceTableLoanSimulation.UseCase.Interfaces;

namespace PrjPriceTableLoanSimulation.UseCase.UseCases.DeleteSimulate
{
    public class DeleteSimulateRequestHandler : IRequestHandler<DeleteSimulateRequest, Unit>
    {
        private readonly IGenericRepository<Proposal, int> _proposalRepository;
        private readonly Resources _resources;

        public DeleteSimulateRequestHandler(IGenericRepository<Proposal, int> proposalRepository, Resources resources)
        {
            _proposalRepository = proposalRepository;
            _resources = resources;
        }

        public async Task<Unit> Handle(DeleteSimulateRequest request, CancellationToken cancellationToken)
        {
            var result = await _proposalRepository.Queryable(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (result == null)
                throw new ConflictException(_resources.ProposalNotExists());

            await _proposalRepository.DeleteAsync(result);

            return Unit.Value;
        }
    }
}
