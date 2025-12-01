

using RestAPI.Application.Commands;
using RestAPI.Application.Queries;
using RestAPI.Models;
using RestAPI.Repositories;
using RestAPI.Exceptions; 
using RestAPI.Services;   
using MediatR;
using AutoMapper;

namespace RestAPI.Application.Handlers
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, CreateBookResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger; 

        public CreateBookCommandHandler(
            IBookRepository bookRepository, 
            IAuthorRepository authorRepository, 
            IMapper mapper,
            ILoggingService logger) 
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<CreateBookResult> Handle(CreateBookCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating book: {Title}", command.Title);
                if (string.IsNullOrEmpty(command.Title))
                {
                    throw new ValidationException("Title", "Book title is required");
                }

                // Check author exists - throw NotFoundException
                var author = _authorRepository.GetById(command.AuthorId);
                if (author == null)
                {
                    throw new NotFoundException("Author", command.AuthorId);
                }

                //  Check duplicate - throw ConflictException
                var existingBook = _bookRepository.GetByTitle(command.Title);
                if (existingBook != null)
                {
                    throw new ConflictException($"Book with title '{command.Title}' already exists");
                }

                // Create book
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

                _logger.LogInformation("Book created successfully: {BookId}", newBook.BookId);

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
            catch (Exception ex) when (ex is not AppException)
            {
                //  Log unexpected errors and rethrow
                _logger.LogError(ex, "Unexpected error creating book: {Title}", command.Title);
                throw; // Middleware will catch and handle
            }
        }
    }

    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, UpdateBookResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public UpdateBookCommandHandler(
            IBookRepository bookRepository, 
            IAuthorRepository authorRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<UpdateBookResult> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating book: {BookId}", command.BookId);

                //  Throw NotFoundException if book doesn't exist
                var book = _bookRepository.GetById(command.BookId);
                if (book == null)
                {
                    throw new NotFoundException("Book", command.BookId);
                }

                // Validate author if being updated
                if (command.NewAuthorId.HasValue)
                {
                    var author = _authorRepository.GetById(command.NewAuthorId.Value);
                    if (author == null)
                    {
                        throw new NotFoundException("Author", command.NewAuthorId.Value);
                    }
                    book.AuthorId = command.NewAuthorId.Value;
                }

                // Update fields
                if (command.NewTitle != null)
                    book.Title = command.NewTitle;
                
                if (command.NewPublisher != null)
                    book.Publisher = command.NewPublisher;
                
                if (command.NewBarcode != null)
                    book.Barcode = command.NewBarcode;
                
                if (command.NewISBN != null)
                    book.ISBN = command.NewISBN;
                
                if (command.NewSubjectGenre != null)
                    book.SubjectGenre = command.NewSubjectGenre;
                
                if (command.NewPublicationDate.HasValue)
                    book.PublicationDate = command.NewPublicationDate;

                _bookRepository.Update(book);

                _logger.LogInformation("Book updated successfully: {BookId}", command.BookId);

                var updatedBook = _bookRepository.GetById(book.BookId);
                var bookData = _mapper.Map<UpdateBookResult.BookData>(updatedBook);

                return Task.FromResult(new UpdateBookResult
                {
                    Success = true,
                    Message = "Book updated successfully!",
                    UpdatedBook = bookData
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error updating book: {BookId}", command.BookId);
                throw;
            }
        }
    }

    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, DeleteBookResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoggingService _logger;

        public DeleteBookCommandHandler(IBookRepository bookRepository, ILoggingService logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public Task<DeleteBookResult> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Deleting book: {BookId}", command.BookId);

                // Throw NotFoundException
                var book = _bookRepository.GetById(command.BookId);
                if (book == null)
                {
                    throw new NotFoundException("Book", command.BookId);
                }

                _bookRepository.Delete(command.BookId);

                _logger.LogInformation("Book deleted successfully: {BookId}", command.BookId);

                return Task.FromResult(new DeleteBookResult
                {
                    Success = true,
                    Message = "Book deleted successfully!"
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error deleting book: {BookId}", command.BookId);
                throw;
            }
        }
    }

    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, GetBookResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public GetBookByIdQueryHandler(
            IBookRepository bookRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<GetBookResult> Handle(GetBookByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var book = _bookRepository.GetById(query.BookId);
                
                //  Throw NotFoundException
                if (book == null)
                {
                    throw new NotFoundException("Book", query.BookId);
                }

                var bookData = _mapper.Map<GetBookResult.BookData>(book);

                return Task.FromResult(new GetBookResult
                {
                    Success = true,
                    Message = "Book found",
                    Book = bookData
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error getting book: {BookId}", query.BookId);
                throw;
            }
        }
    }

    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, GetAllBooksResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public GetAllBooksQueryHandler(
            IBookRepository bookRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<GetAllBooksResult> Handle(GetAllBooksQuery query, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching all books");

                var books = _bookRepository.GetAll();
                var bookSummaries = _mapper.Map<List<GetAllBooksResult.BookSummary>>(books);

                _logger.LogInformation("Retrieved {Count} books", books.Count);

                return Task.FromResult(new GetAllBooksResult
                {
                    Success = true,
                    Books = bookSummaries
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching all books");
                throw;
            }
        }
    }

    public class GetBooksByAuthorQueryHandler : IRequestHandler<GetBooksByAuthorQuery, GetAllBooksResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public GetBooksByAuthorQueryHandler(
            IBookRepository bookRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<GetAllBooksResult> Handle(GetBooksByAuthorQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var books = _bookRepository.GetByAuthorId(query.AuthorId);
                var bookSummaries = _mapper.Map<List<GetAllBooksResult.BookSummary>>(books);

                return Task.FromResult(new GetAllBooksResult
                {
                    Success = true,
                    Books = bookSummaries
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching books for author: {AuthorId}", query.AuthorId);
                throw;
            }
        }
    }
}