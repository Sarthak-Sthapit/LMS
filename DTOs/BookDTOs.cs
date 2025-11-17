namespace RestAPI.DTOs
{
    public class CreateBookDto
    {
        public string Title { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string Publisher { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string SubjectGenre { get; set; } = string.Empty;
        public DateTime? PublicationDate { get; set; }
    }

    public class UpdateBookDto
    {
        public string? NewTitle { get; set; }
        public int? NewAuthorId { get; set; }
        public string? NewPublisher { get; set; }
        public string? NewBarcode { get; set; }
        public string? NewISBN { get; set; }
        public string? NewSubjectGenre { get; set; }
        public DateTime? NewPublicationDate { get; set; }
    }
}