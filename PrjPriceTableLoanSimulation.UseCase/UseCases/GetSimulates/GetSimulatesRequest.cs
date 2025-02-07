using MediatR;

namespace PrjPriceTableLoanSimulation.UseCase.UseCases.GetSimulates
{
    public class GetSimulatesRequest : IRequest<GetSimulatesResponse>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
