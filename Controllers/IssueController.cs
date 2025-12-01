using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RestAPI.DTOs;
using RestAPI.Application.Commands;
using RestAPI.Application.Queries;
using MediatR;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IssueController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IssueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //  Checkout Book
        [HttpPost("checkout")]
        public IActionResult CheckoutBook([FromBody] CheckoutBookDto dto)
        {
            var command = new CheckoutBookCommand
            {
                BookId = dto.BookId,
                StudentId = dto.StudentId,
                BorrowDays = dto.BorrowDays ?? 14
            };

            var result = _mediator.Send(command).Result;

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(new
            {
                message = result.Message,
                issue = result.Issue
            });
        }

        //  Return Book
        [HttpPost("return/{issueId}")]
        public IActionResult ReturnBook(int issueId)
        {
            var command = new ReturnBookCommand { IssueId = issueId };
            var result = _mediator.Send(command).Result;

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(new
            {
                message = result.Message,
                daysOverdue = result.DaysOverdue,
                issue = result.Issue
            });
        }

        //  Get Active Loans
        [HttpGet("active")]
        public IActionResult GetActiveLoans()
        {
            var query = new GetActiveLoansQuery();
            var result = _mediator.Send(query).Result;
            return Ok(result.Loans);
        }

        // Get Overdue Books
        [HttpGet("overdue")]
        public IActionResult GetOverdueBooks()
        {
            var query = new GetOverdueBooksQuery();
            var result = _mediator.Send(query).Result;
            return Ok(result.Loans);
        }

        // Keep existing endpoints
        [HttpGet]
        public IActionResult GetAllIssues()
        {
            var query = new GetAllIssuesQuery();
            var result = _mediator.Send(query).Result;
            return Ok(result.Issues);
        }

        [HttpGet("{id}")]
        public IActionResult GetIssueById(int id)
        {
            var query = new GetIssueByIdQuery { IssueId = id };
            var result = _mediator.Send(query).Result;

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Issue);
        }

        [HttpGet("student/{studentId}")]
        public IActionResult GetIssuesByStudent(int studentId)
        {
            var query = new GetIssuesByStudentQuery { StudentId = studentId };
            var result = _mediator.Send(query).Result;
            return Ok(result.Issues);
        }

        [HttpGet("book/{bookId}")]
        public IActionResult GetIssuesByBook(int bookId)
        {
            var query = new GetIssuesByBookQuery { BookId = bookId };
            var result = _mediator.Send(query).Result;
            return Ok(result.Issues);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteIssue(int id)
        {
            var command = new DeleteIssueCommand { IssueId = id };
            var result = _mediator.Send(command).Result;

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(new { message = result.Message });
        }
    }
}