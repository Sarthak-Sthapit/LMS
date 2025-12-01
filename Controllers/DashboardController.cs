
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RestAPI.Repositories;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IIssueRepository _issueRepository;

        public DashboardController(
            IBookRepository bookRepository,
            IStudentRepository studentRepository,
            IIssueRepository issueRepository)
        {
            _bookRepository = bookRepository;
            _studentRepository = studentRepository;
            _issueRepository = issueRepository;
        }

        
    }
}
