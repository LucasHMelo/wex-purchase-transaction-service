using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;
using Wex.TransactionManager.Domain.Entities;
using Wex.TransactionManager.Domain.Repositories;

namespace Wex.TransactionManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController(ITransactionRepository mediator) : ControllerBase
    {
        private readonly ITransactionRepository _mediator = mediator;

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(
            [FromBody] CreateTransactionInput input,
            CancellationToken cancellationToken
        )
        {

            Transaction t = new Transaction(input.Description,
                input.Amount,
                input.TransactionDate);

            await _mediator.Insert(t, cancellationToken);
            return CreatedAtAction(
                nameof(Create),
                new { test = "Guid" },
                Guid.NewGuid
            );
        }
    }
}
