using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;
using Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;
using Wex.TransactionManager.Domain.Entities;
using Wex.TransactionManager.Domain.Repositories;

namespace Wex.TransactionManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

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
            var output = await _mediator.Send(new GetTransactionInput(id, currency), cancellationToken);
            return Ok(output);
        }

    }
}
