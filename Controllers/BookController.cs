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
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] CreateBookDto dto)
        {
            var command = new CreateBookCommand
            {
                Title = dto.Title,
                AuthorId = dto.AuthorId,
                Publisher = dto.Publisher,
                Barcode = dto.Barcode,
                ISBN = dto.ISBN,
                SubjectGenre = dto.SubjectGenre,
                PublicationDate = dto.PublicationDate
            };

            var result = _mediator.Send(command).Result;
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(new
            {
                message = result.Message,
                book = result.Book
            });
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var query = new GetAllBooksQuery();
            var result = _mediator.Send(query).Result;
            return Ok(result.Books);
        }

        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var query = new GetBookByIdQuery { BookId = id };
            var result = _mediator.Send(query).Result;
            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Book);
        }

        [HttpGet("author/{authorId}")]
        public IActionResult GetBooksByAuthor(int authorId)
        {
            var query = new GetBooksByAuthorQuery { AuthorId = authorId };
            var result = _mediator.Send(query).Result;
            return Ok(result.Books);
        }

    
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] UpdateBookDto dto)
        {
            

            var command = new UpdateBookCommand
            {
                BookId = id,
                NewTitle = dto.NewTitle,
                NewAuthorId = dto.NewAuthorId,
                NewPublisher = dto.NewPublisher,
                NewBarcode = dto.NewBarcode,
                NewISBN = dto.NewISBN,
                NewSubjectGenre = dto.NewSubjectGenre,
                NewPublicationDate = dto.NewPublicationDate
            };

        
            var result = _mediator.Send(command).Result;
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(new
            {
                message = result.Message,
                book = result.UpdatedBook
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var command = new DeleteBookCommand { BookId = id };
            var result = _mediator.Send(command).Result;

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(new { message = result.Message });
        }
    }
}