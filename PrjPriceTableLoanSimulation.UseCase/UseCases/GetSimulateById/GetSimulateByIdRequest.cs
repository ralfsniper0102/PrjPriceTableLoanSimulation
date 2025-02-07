using MediatR;

namespace PrjPriceTableLoanSimulation.UseCase.UseCases.GetSimulateById
{
    public class GetSimulateByIdRequest : IRequest<GetSimulateByIdResponse>
    {
        public int Id { get; set; }
    }
}
