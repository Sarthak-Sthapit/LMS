using MediatR;

namespace RestAPI.Application.Commands
{
    public class UpdateBookCommand : IRequest<UpdateBookResult>
    {
        public int BookId { get; set; }
        public string? NewTitle { get; set; }
        public int? NewAuthorId { get; set; }
        public string? NewPublisher { get; set; }
        public string? NewBarcode { get; set; }
        public string? NewISBN { get; set; }
        public string? NewSubjectGenre { get; set; }
        public DateTime? NewPublicationDate { get; set; }
    }

    public class UpdateBookResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public BookData? UpdatedBook { get; set; }
        
        public class BookData
        {
            public int BookId { get; set; }
            public string Title { get; set; } = string.Empty;
            public int AuthorId { get; set; }
            public string AuthorName { get; set; } = string.Empty;
            public string Publisher { get; set; } = string.Empty;
            public string Barcode { get; set; } = string.Empty;
            public string ISBN { get; set; } = string.Empty;
            public string SubjectGenre { get; set; } = string.Empty;
            public DateTime? PublicationDate { get; set; }
        }
    }
}