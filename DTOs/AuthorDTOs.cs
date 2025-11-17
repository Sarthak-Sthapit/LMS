namespace RestAPI.DTOs
{
    // DTO for creating an author
    public class CreateAuthorDto
    {
        public string AuthorName { get; set; } = string.Empty;
    }

    // DTO for updating an author
    public class UpdateAuthorDto
    {
        public string? NewAuthorName { get; set; }
    }
}