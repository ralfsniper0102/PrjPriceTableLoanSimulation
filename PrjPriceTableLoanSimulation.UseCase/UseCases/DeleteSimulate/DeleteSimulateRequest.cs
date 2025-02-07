using MediatR;

namespace PrjPriceTableLoanSimulation.UseCase.UseCases.DeleteSimulate
{
    public class DeleteSimulateRequest : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
