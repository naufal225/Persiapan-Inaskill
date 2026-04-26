using _005_Expense_Tracker_API_01.DTOs;
using _005_Expense_Tracker_API_01.Models;
using _005_Expense_Tracker_API_01.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _005_Expense_Tracker_API_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactionsService;

        public TransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<Transaction>>> GetAll([FromQuery] GetTransactionQuery query)
        {
            if(query.Page < 1)
            {
                return BadRequestError("Page cannot be less than 1.");
            }

            if(query.PageSize < 1)
            {
                return BadRequestError("Page sizecannot be less than 1.");
            }

            if(query.PageSize > 100)
            {
                return BadRequestError("Page size cannot be more than 100.");
            }

            PaginatedResponse<Transaction> response = await _transactionsService.GetAll(query);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetById(int id)
        {
            Transaction? transaction = await _transactionsService.GetById(id);
            if(transaction == null)
            {
                return NotFoundError($"Transaction with id {id} cannot be found");
            }

            return Ok(transaction);
        }

        [HttpGet("summary")]
        public async Task<ActionResult<SummaryResponse>> GetSummary()
        {
            SummaryResponse response = await _transactionsService.GetSummary();

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> Create(CreateTransactionRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequestError("Title cannot be empty.");
            }

            if(request.Amount < 1)
            {
                return BadRequestError("Amount cannot be less than 1.");
            } 

            if(request.Type.ToLower().Trim() != "expense" && request.Type.ToLower().Trim() != "income")
            {
                return BadRequestError($"Request type must be 'expense' or 'income', not {(!string.IsNullOrWhiteSpace(request.Type) ? request.Type : "empty")}");
            }

            if(string.IsNullOrWhiteSpace(request.Category))
            {
                return BadRequestError("Category cannot be empty");
            }

            Transaction transaction = await _transactionsService.Create(request);

            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Transaction>> Update(int id, UpdateTransactionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequestError("Title cannot be empty.");
            }

            if (request.Amount < 1)
            {
                return BadRequestError("Amount cannot be less than 1.");
            }

            if (request.Type.ToLower().Trim() != "expense" && request.Type.ToLower().Trim() != "income")
            {
                return BadRequestError($"Request type must be 'expense' or 'income', not {(!string.IsNullOrWhiteSpace(request.Type) ? request.Type : "empty")}");
            }

            if (string.IsNullOrWhiteSpace(request.Category))
            {
                return BadRequestError("Category cannot be empty");
            }

            Transaction? transaction = await _transactionsService.Update(id, request);
            if(transaction == null)
            {
                return NotFoundError($"Transaction with id {id} cannot be found");
            }

            return Ok(transaction);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool delete = await _transactionsService.Delete(id);
            if (delete == false)
            {
                return NotFoundError($"Transaction with id {id} cannot be found");
            }

            return NoContent();
        }

        private BadRequestObjectResult BadRequestError(string message)
        {
            return BadRequest(new ErrorResponse
            {
                Message = message
            });
        }

        private NotFoundObjectResult NotFoundError(string message)
        {
            return NotFound(new ErrorResponse
            {
                Message = message
            });
        }
    }
}
