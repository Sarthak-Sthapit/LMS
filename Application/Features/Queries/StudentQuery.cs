using MediatR;

namespace RestAPI.Application.Queries
{
    public class GetStudentByIdQuery : IRequest<GetStudentResult>
    {
        public int StudentId { get; set; }
    }

    public class GetAllStudentsQuery : IRequest<GetAllStudentsResult>
    {
    }

    public class GetStudentResult
    {
        public bool Success { get; set; }
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

    public class GetAllStudentsResult
    {
        public bool Success { get; set; }
        public List<StudentSummary> Students { get; set; } = new();
        
        public class StudentSummary
        {
            public int StudentId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public string Faculty { get; set; } = string.Empty;
            public string Semester { get; set; } = string.Empty;
            public string ContactNo { get; set; } = string.Empty;
        }
    }
}