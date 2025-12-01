using MediatR;

namespace RestAPI.Application.Queries
{
    public class GetBookByIdQuery : IRequest<GetBookResult>
    {
        public int BookId { get; set; }
    }

    public class GetAllBooksQuery : IRequest<GetAllBooksResult>
    {
    }

    public class GetBooksByAuthorQuery : IRequest<GetAllBooksResult>
    {
        public int AuthorId { get; set; }
    }

    public class GetBookResult
    {
        public bool Success { get; set; }
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

    public class GetAllBooksResult
    {
        public bool Success { get; set; }
        public List<BookSummary> Books { get; set; } = new();
        

        public class BookSummary
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