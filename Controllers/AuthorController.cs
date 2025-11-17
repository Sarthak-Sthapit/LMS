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
    public class AuthorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody] CreateAuthorDto dto)
        {
            var command = new CreateAuthorCommand
            {
                AuthorName = dto.AuthorName
            };

            var result = _mediator.Send(command).Result;

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(new
            {
                message = result.Message,
                author = result.Author
            });
        }

        [HttpGet]
        public IActionResult GetAllAuthors()
        {
            var query = new GetAllAuthorsQuery();
            var result = _mediator.Send(query).Result;
            return Ok(result.Authors);
        }

        [HttpGet("{id}")]
        public IActionResult GetAuthorById(int id)
        {
            var query = new GetAuthorByIdQuery { AuthorId = id };
            var result = _mediator.Send(query).Result;

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Author);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAuthor(int id, [FromBody] UpdateAuthorDto dto)
        {
            var command = new UpdateAuthorCommand  
            {
                AuthorId = id,
                NewAuthorName = dto.NewAuthorName
            };

            var result = _mediator.Send(command).Result;
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(new { 
                message = result.Message, 
                author = result.UpdatedAuthor  
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            var command = new DeleteAuthorCommand { AuthorId = id };
            var result = _mediator.Send(command).Result;

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(new { message = result.Message });
        }
    }
}