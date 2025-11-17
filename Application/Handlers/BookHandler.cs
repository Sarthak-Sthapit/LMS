using RestAPI.Application.Commands;
using RestAPI.Application.Queries;
using RestAPI.Models;
using RestAPI.Repositories;
using MediatR;
using AutoMapper;

namespace RestAPI.Application.Handlers
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, CreateBookResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public CreateBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public Task<CreateBookResult> Handle(CreateBookCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Title))
            {
                return Task.FromResult(new CreateBookResult
                {
                    Success = false,
                    Message = "Book title is required"
                });
            }

            var author = _authorRepository.GetById(command.AuthorId);
            if (author == null)
            {
                return Task.FromResult(new CreateBookResult
                {
                    Success = false,
                    Message = "Author not found!"
                });
            }

            var existingBook = _bookRepository.GetByTitle(command.Title);
            if (existingBook != null)
            {
                return Task.FromResult(new CreateBookResult
                {
                    Success = false,
                    Message = "Book already exists!"
                });
            }

            var newBook = new Book
            {
                Title = command.Title,
                AuthorId = command.AuthorId,
                Publisher = command.Publisher,
                Barcode = command.Barcode,
                ISBN = command.ISBN,
                SubjectGenre = command.SubjectGenre,
                PublicationDate = command.PublicationDate,
                IsDeleted = false
            };

            _bookRepository.Add(newBook);

            var bookWithAuthor = _bookRepository.GetById(newBook.BookId);
            var bookData = _mapper.Map<CreateBookResult.BookData>(bookWithAuthor);

            return Task.FromResult(new CreateBookResult
            {
                Success = true,
                BookId = newBook.BookId,
                Message = "Book created successfully!",
                Book = bookData
            });
        }
    }

    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, UpdateBookResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public UpdateBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public Task<UpdateBookResult> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
        {
            var book = _bookRepository.GetById(command.BookId);
            if (book == null)
            {
                return Task.FromResult(new UpdateBookResult
                {
                    Success = false,
                    Message = "Book not found!"
                });
            }

            if (!string.IsNullOrEmpty(command.NewTitle))
                book.Title = command.NewTitle;

            if (command.NewAuthorId.HasValue)
            {
                var author = _authorRepository.GetById(command.NewAuthorId.Value);
                if (author == null)
                {
                    return Task.FromResult(new UpdateBookResult
                    {
                        Success = false,
                        Message = "Author not found!"
                    });
                }
                book.AuthorId = command.NewAuthorId.Value;
            }
            if (!string.IsNullOrEmpty(command.NewPublisher))
                book.Publisher = command.NewPublisher;
            
            if (!string.IsNullOrEmpty(command.NewBarcode))
                book.Barcode = command.NewBarcode;
            
            if (!string.IsNullOrEmpty(command.NewISBN))
                book.ISBN = command.NewISBN;
            
            if (!string.IsNullOrEmpty(command.NewSubjectGenre))
                book.SubjectGenre = command.NewSubjectGenre;
            
            if (command.NewPublicationDate.HasValue)
                book.PublicationDate = command.NewPublicationDate;

            _bookRepository.Update(book);

            var updatedBook = _bookRepository.GetById(book.BookId);
            var bookData = _mapper.Map<UpdateBookResult.BookData>(updatedBook);

            return Task.FromResult(new UpdateBookResult
            {
                Success = true,
                Message = "Book updated successfully!",
                UpdatedBook = bookData
            });
        }
    }

    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, DeleteBookResult>
    {
        private readonly IBookRepository _bookRepository;

        public DeleteBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<DeleteBookResult> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
        {
            var book = _bookRepository.GetById(command.BookId);

            if (book == null)
            {
                return Task.FromResult(new DeleteBookResult
                {
                    Success = false,
                    Message = "Book not found!"
                });
            }

            _bookRepository.Delete(command.BookId);

            return Task.FromResult(new DeleteBookResult
            {
                Success = true,
                Message = "Book deleted successfully!"
            });
        }
    }

    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, GetBookResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public GetBookByIdQueryHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public Task<GetBookResult> Handle(GetBookByIdQuery query, CancellationToken cancellationToken)
        {
            var book = _bookRepository.GetById(query.BookId);
            
            if (book == null)
            {
                return Task.FromResult(new GetBookResult
                {
                    Success = false,
                    Message = "Book not found!"
                });
            }

            var bookData = _mapper.Map<GetBookResult.BookData>(book);

            return Task.FromResult(new GetBookResult
            {
                Success = true,
                Message = "Book found",
                Book = bookData
            });
        }
    }

    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, GetAllBooksResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public GetAllBooksQueryHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public Task<GetAllBooksResult> Handle(GetAllBooksQuery query, CancellationToken cancellationToken)
        {
            var books = _bookRepository.GetAll();
            var bookSummaries = _mapper.Map<List<GetAllBooksResult.BookSummary>>(books);

            return Task.FromResult(new GetAllBooksResult
            {
                Success = true,
                Books = bookSummaries
            });
        }
    }

    public class GetBooksByAuthorQueryHandler : IRequestHandler<GetBooksByAuthorQuery, GetAllBooksResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public GetBooksByAuthorQueryHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public Task<GetAllBooksResult> Handle(GetBooksByAuthorQuery query, CancellationToken cancellationToken)
        {
            var books = _bookRepository.GetByAuthorId(query.AuthorId);
            var bookSummaries = _mapper.Map<List<GetAllBooksResult.BookSummary>>(books);

            return Task.FromResult(new GetAllBooksResult
            {
                Success = true,
                Books = bookSummaries
            });
        }
    }
}