using MediatR;

namespace RestAPI.Application.Commands
{
    public class UpdateStudentCommand : IRequest<UpdateStudentResult>
    {
        public int StudentId { get; set; }
        public string? NewName { get; set; }
        public string? NewAddress { get; set; }
        public string? NewContactNo { get; set; }
        public string? NewFaculty { get; set; }
        public string? NewSemester { get; set; }
    }

    public class UpdateStudentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public StudentData? UpdatedStudent { get; set; }
        
        public class StudentData
        {
            public int StudentId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public string ContactNo { get; set; } = string.Empty;
            public string Faculty { get; set; } = string.Empty;
            public string Semester { get; set; } = string.Empty;
        }
    }
}