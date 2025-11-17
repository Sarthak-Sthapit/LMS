namespace RestAPI.DTOs
{
    public class CreateStudentDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactNo { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
    }

    public class UpdateStudentDto
    {
        public string? NewName { get; set; }
        public string? NewAddress { get; set; }
        public string? NewContactNo { get; set; }
        public string? NewFaculty { get; set; }
        public string? NewSemester { get; set; }
    }
}