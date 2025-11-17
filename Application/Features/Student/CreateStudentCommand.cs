using MediatR;

namespace RestAPI.Application.Commands
{
    public class CreateStudentCommand : IRequest<CreateStudentResult>
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactNo { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
    }

    public class CreateStudentResult
    {
        public bool Success { get; set; }
        public int StudentId { get; set; }
        public string Message { get; set; } = string.Empty;
        public StudentData? Student { get; set; }

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