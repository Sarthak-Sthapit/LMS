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
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public IActionResult CreateStudent([FromBody] CreateStudentDto dto)
        {
            var command = new CreateStudentCommand
            {
                Name = dto.Name,
                Address = dto.Address,
                ContactNo = dto.ContactNo,
                Faculty = dto.Faculty,
                Semester = dto.Semester
            };

            var result = _mediator.Send(command).Result;

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(new
            {
                message = result.Message,
                student = result.Student
            });
        }

        [HttpGet]
        public IActionResult GetAllStudents()
        {
            var query = new GetAllStudentsQuery();
            var result = _mediator.Send(query).Result;
            return Ok(result.Students);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var query = new GetStudentByIdQuery { StudentId = id };
            var result = _mediator.Send(query).Result;

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Student);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] UpdateStudentDto dto)
        {
            var command = new UpdateStudentCommand
            {
                StudentId = id,
                NewName = dto.NewName,
                NewAddress = dto.NewAddress,
                NewContactNo = dto.NewContactNo,
                NewFaculty = dto.NewFaculty,
                NewSemester = dto.NewSemester
            };

            var result = _mediator.Send(command).Result;
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(new { 
                message = result.Message, 
                student = result.UpdatedStudent  
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var command = new DeleteStudentCommand { StudentId = id };
            var result = _mediator.Send(command).Result;

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(new { message = result.Message });
        }
    }
}