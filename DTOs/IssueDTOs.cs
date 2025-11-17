namespace RestAPI.DTOs
{
    public class CreateIssueDto
    {
        public int BookId { get; set; }
        public int StudentId { get; set; }
    }

    public class UpdateIssueDto
    {
        public int? NewBookId { get; set; }
        public int? NewStudentId { get; set; }
    }
}