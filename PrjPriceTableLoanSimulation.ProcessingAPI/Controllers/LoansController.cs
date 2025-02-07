using MediatR;
using Microsoft.AspNetCore.Mvc;
using PrjPriceTableLoanSimulation.UseCase.UseCases.DeleteSimulate;
using PrjPriceTableLoanSimulation.UseCase.UseCases.GetSimulateById;
using PrjPriceTableLoanSimulation.UseCase.UseCases.GetSimulates;
using PrjPriceTableLoanSimulation.UseCase.UseCases.Simulate;
using PrjPriceTableLoanSimulation.UseCase.UseCases.UpdateSimulate;
using System.Net;

namespace PrjPriceTableLoanSimulation.ProcessingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : BaseApiController<LoansController>
    {
        private readonly IMediator _mediator;

        public LoansController(IMediator mediator, Serilog.ILogger logger) : base(logger, mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Simulate")]
        [ProducesResponseType(typeof(SimulateResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateSimulate([FromBody] SimulateRequest request)
        {
            return await CreateActionResult(request);
        }

        [HttpGet("GetSimulates")]
        [ProducesResponseType(typeof(GetSimulatesResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSimulates(string search = "", int page = 1, int pageSize = 10)
        {
            return await CreateActionResult(new GetSimulatesRequest { Page = page, PageSize = pageSize });
        }

        [HttpGet("GetSimulateById/{id}")]
        [ProducesResponseType(typeof(GetSimulateByIdResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSimulateById(int id)
        {
            return await CreateActionResult(new GetSimulateByIdRequest { Id = id });
        }

        [HttpPut("UpdateSimulate")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateSimulate([FromBody] UpdateSimulateRequest request)
        {
            return await CreateActionResult(request);
        }

        [HttpDelete("DeleteSimulate/{id}")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteSimulate(int id)
        {
            return await CreateActionResult(new DeleteSimulateRequest { Id = id });
        }

    }
}
