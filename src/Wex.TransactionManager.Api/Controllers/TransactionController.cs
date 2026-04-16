using Microsoft.AspNetCore.Mvc;
using Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;

namespace Wex.TransactionManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
            => _mediator = mediator;

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(
            [FromBody] CreateTransactionInput input,
            CancellationToken cancellationToken
        )
        {
            var output = await _mediator.Send(input, cancellationToken);
            return CreatedAtAction(
                nameof(Create),
                new { output.Id },
                output
            );
        }
    }
}
