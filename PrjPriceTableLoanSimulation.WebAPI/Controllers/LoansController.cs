using MediatR;
using Microsoft.AspNetCore.Mvc;
using PrjPriceTableLoanSimulation.Messaging;
using PrjPriceTableLoanSimulation.UseCase.UseCases.DeleteSimulate;
using PrjPriceTableLoanSimulation.UseCase.UseCases.GetSimulateById;
using PrjPriceTableLoanSimulation.UseCase.UseCases.GetSimulates;
using PrjPriceTableLoanSimulation.UseCase.UseCases.Simulate;
using PrjPriceTableLoanSimulation.UseCase.UseCases.UpdateSimulate;
using System.Net;

namespace PrjPriceTableLoanSimulation.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : BaseController<LoansController>
    {
        public LoansController(RabbitMqClientService rabbitMqService, Serilog.ILogger logger) : base(rabbitMqService, logger)
        {
        }

        [HttpPost("Simulate")]
        [ProducesResponseType(typeof(SimulateResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateSimulate([FromBody] SimulateRequest request)
        {
            return await HandleRequestAsync<SimulateRequest, SimulateResponse>(request);
        }

        [HttpGet("GetSimulates")]
        [ProducesResponseType(typeof(GetSimulatesResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSimulates(int page = 1, int pageSize = 10)
        {
            var request = new GetSimulatesRequest { Page = page, PageSize = pageSize };

            return await HandleRequestAsync<GetSimulatesRequest, GetSimulatesResponse>(request);
        }

        [HttpGet("GetSimulateById/{id}")]
        [ProducesResponseType(typeof(GetSimulateByIdResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSimulateById(int id)
        {
            var request = new GetSimulateByIdRequest { Id = id };

            return await HandleRequestAsync<GetSimulateByIdRequest, GetSimulateByIdResponse>(request);
        }

        [HttpPut("UpdateSimulate")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateSimulate([FromBody] UpdateSimulateRequest request)
        {
            return await HandleRequestAsync<UpdateSimulateRequest, Unit>(request);
        }

        [HttpDelete("DeleteSimulate/{id}")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteSimulate(int id)
        {
            return await HandleRequestAsync<DeleteSimulateRequest, Unit>(new DeleteSimulateRequest { Id = id });
        }
    }
}
