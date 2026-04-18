using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;
using Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;

namespace Wex.TransactionManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController(IMediator mediator, ILogger<TransactionController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
         private readonly ILogger<TransactionController> _logger = logger;

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(
            [FromBody] CreateTransactionInput transaction,
            CancellationToken cancellationToken
        )
        {

            var response = await _mediator.Send(transaction, cancellationToken);
            return CreatedAtAction(
                nameof(Create),
                new { response.TransactionId },
                response
            );
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(GetTransactionOutput), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id,
            CancellationToken cancellationToken,
            [FromQuery] string currency = "Brazil-Real"
        )
        {
            _logger.LogInformation("Retrieving Transaction {TransactionId} with currency conversion to {TargetCurrency}", id, currency);
            
            var output = await _mediator.Send(new GetTransactionInput(id, currency), cancellationToken);
            return Ok(output);
        }

    }
}
