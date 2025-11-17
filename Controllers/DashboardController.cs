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

        [HttpGet("statistics")]
        public IActionResult GetStatistics()
        {
            var allIssues = _issueRepository.GetAll();
            var allStudents = _studentRepository.GetAll();
            
            var borrowed = allIssues.Count;
            var overdue = allIssues.Count(i => i.IssueDate < DateTime.UtcNow.AddDays(-14)); // 14 days overdue
            var visitors = allStudents.Count; // Assuming visitors = total students
            var newMembers = allStudents.Count(s => s.StudentId > 0); // Mock data

            return Ok(new
            {
                borrowed,
                overdue,
                visitors,
                newMembers
            });
        }

        [HttpGet("chart-data")]
        public IActionResult GetChartData()
        {
            // Mock data 
            var chartData = new List<object>();
            
            for (int month = 1; month <= 12; month++)
            {
                chartData.Add(new
                {
                    month = month.ToString("D2"),
                    visitors = new Random().Next(30, 100),
                    borrowers = new Random().Next(20, 80)
                });
            }

            return Ok(chartData);
        }
    }
}