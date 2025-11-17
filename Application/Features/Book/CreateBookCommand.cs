using MediatR;

namespace RestAPI.Application.Commands
{
    public class CreateBookCommand : IRequest<CreateBookResult>
    {
        public string Title { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string Publisher { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string SubjectGenre { get; set; } = string.Empty;
        public DateTime? PublicationDate { get; set; }
    }

    public class CreateBookResult
    {
        public bool Success { get; set; }
        public int BookId { get; set; }
        public string Message { get; set; } = string.Empty;
        public BookData? Book { get; set; }

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