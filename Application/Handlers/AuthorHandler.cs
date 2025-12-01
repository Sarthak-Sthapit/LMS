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
    // creating author
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, CreateAuthorResult>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public CreateAuthorCommandHandler(
            IAuthorRepository authorRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<CreateAuthorResult> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating author: {AuthorName}", command.AuthorName);

                // Validation
                if (string.IsNullOrEmpty(command.AuthorName))
                {
                    throw new ValidationException("AuthorName", "Author name is required");
                }

                // Check for duplicate
                var existingAuthor = _authorRepository.GetByName(command.AuthorName);
                if (existingAuthor != null)
                {
                    throw new ConflictException($"Author '{command.AuthorName}' already exists");
                }

                // Create new author
                var newAuthor = new Author
                {
                    AuthorName = command.AuthorName,
                    IsDeleted = false
                };

                _authorRepository.Add(newAuthor);

                _logger.LogInformation("Author created successfully: {AuthorId}", newAuthor.AuthorId);

                var authorData = _mapper.Map<CreateAuthorResult.AuthorData>(newAuthor);

                return Task.FromResult(new CreateAuthorResult
                {
                    Success = true,
                    AuthorId = newAuthor.AuthorId,
                    Message = "Author created successfully!",
                    Author = authorData
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error creating author: {AuthorName}", command.AuthorName);
                throw;
            }
        }
    }

    // ==================== UPDATE AUTHOR ====================
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, UpdateAuthorResult>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public UpdateAuthorCommandHandler(
            IAuthorRepository authorRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<UpdateAuthorResult> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating author: {AuthorId}", command.AuthorId);

                // Check if author exists
                var author = _authorRepository.GetById(command.AuthorId);
                if (author == null)
                {
                    throw new NotFoundException("Author", command.AuthorId);
                }

                // Check for duplicate name
                if (!string.IsNullOrEmpty(command.NewAuthorName) && command.NewAuthorName != author.AuthorName)
                {
                    var existingAuthor = _authorRepository.GetByName(command.NewAuthorName);
                    if (existingAuthor != null)
                    {
                        throw new ConflictException($"Author name '{command.NewAuthorName}' is already taken");
                    }
                }

                // Update
                if (!string.IsNullOrEmpty(command.NewAuthorName))
                    author.AuthorName = command.NewAuthorName;

                _authorRepository.Update(author);

                _logger.LogInformation("Author updated successfully: {AuthorId}", command.AuthorId);

                var authorData = _mapper.Map<UpdateAuthorResult.AuthorData>(author);

                return Task.FromResult(new UpdateAuthorResult
                {
                    Success = true,
                    Message = "Author updated successfully!",
                    UpdatedAuthor = authorData
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error updating author: {AuthorId}", command.AuthorId);
                throw;
            }
        }
    }

    // ==================== DELETE AUTHOR ====================
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, DeleteAuthorResult>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILoggingService _logger;

        public DeleteAuthorCommandHandler(
            IAuthorRepository authorRepository,
            ILoggingService logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public Task<DeleteAuthorResult> Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Deleting author: {AuthorId}", command.AuthorId);

                var author = _authorRepository.GetById(command.AuthorId);
                if (author == null)
                {
                    throw new NotFoundException("Author", command.AuthorId);
                }

                _authorRepository.Delete(command.AuthorId);

                _logger.LogInformation("Author deleted successfully: {AuthorId}", command.AuthorId);

                return Task.FromResult(new DeleteAuthorResult
                {
                    Success = true,
                    Message = "Author deleted successfully!"
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error deleting author: {AuthorId}", command.AuthorId);
                throw;
            }
        }
    }

    // get author by id
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, GetAuthorResult>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public GetAuthorByIdQueryHandler(
            IAuthorRepository authorRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<GetAuthorResult> Handle(GetAuthorByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var author = _authorRepository.GetById(query.AuthorId);
                
                if (author == null)
                {
                    throw new NotFoundException("Author", query.AuthorId);
                }

                var authorData = _mapper.Map<GetAuthorResult.AuthorData>(author);

                return Task.FromResult(new GetAuthorResult
                {
                    Success = true,
                    Message = "Author found",
                    Author = authorData
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error getting author: {AuthorId}", query.AuthorId);
                throw;
            }
        }
    }

    // ==================== GET ALL AUTHORS ====================
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, GetAllAuthorsResult>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public GetAllAuthorsQueryHandler(
            IAuthorRepository authorRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<GetAllAuthorsResult> Handle(GetAllAuthorsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching all authors");

                var authors = _authorRepository.GetAll();
                var authorSummaries = _mapper.Map<List<GetAllAuthorsResult.AuthorSummary>>(authors);

                _logger.LogInformation("Retrieved {Count} authors", authors.Count);

                return Task.FromResult(new GetAllAuthorsResult
                {
                    Success = true,
                    Authors = authorSummaries
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching all authors");
                throw;
            }
        }
    }
}