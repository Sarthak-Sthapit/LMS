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

        [HttpPost]
        public IActionResult CreateIssue([FromBody] CreateIssueDto dto)
        {
            var command = new CreateIssueCommand
            {
                BookId = dto.BookId,
                StudentId = dto.StudentId
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

        [HttpPut("{id}")]
        public IActionResult UpdateIssue(int id, [FromBody] UpdateIssueDto dto)
        {
            var command = new UpdateIssueCommand
            {
                IssueId = id,
                NewBookId = dto.NewBookId,
                NewStudentId = dto.NewStudentId
            };

            var result = _mediator.Send(command).Result;
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(new { 
                message = result.Message, 
                issue = result.UpdatedIssue  
            });
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