using RestAPI.Application.Commands;
using RestAPI.Application.Queries;
using RestAPI.Models;
using RestAPI.Repositories;
using MediatR;
using AutoMapper;

namespace RestAPI.Application.Handlers
{
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, CreateAuthorResult>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public CreateAuthorCommandHandler(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public Task<CreateAuthorResult> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.AuthorName))
            {
                return Task.FromResult(new CreateAuthorResult
                {
                    Success = false,
                    Message = "Author name is required"
                });
            }

            var existingAuthor = _authorRepository.GetByName(command.AuthorName);
            if (existingAuthor != null)
            {
                return Task.FromResult(new CreateAuthorResult
                {
                    Success = false,
                    Message = "Author already exists!"
                });
            }

            var newAuthor = new Author
            {
                AuthorName = command.AuthorName,
                IsDeleted = false
            };

            _authorRepository.Add(newAuthor);

            var authorData = _mapper.Map<CreateAuthorResult.AuthorData>(newAuthor);

            return Task.FromResult(new CreateAuthorResult
            {
                Success = true,
                AuthorId = newAuthor.AuthorId,
                Message = "Author created successfully!",
                Author = authorData  
            });
        }
    }

    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, UpdateAuthorResult>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public UpdateAuthorCommandHandler(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public Task<UpdateAuthorResult> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
        {
            var author = _authorRepository.GetById(command.AuthorId);
            if (author == null)
            {
                return Task.FromResult(new UpdateAuthorResult
                {
                    Success = false,
                    Message = "Author not found!"
                });
            }

            if (!string.IsNullOrEmpty(command.NewAuthorName) && command.NewAuthorName != author.AuthorName)
            {
                var existingAuthor = _authorRepository.GetByName(command.NewAuthorName);
                if (existingAuthor != null)
                {
                    return Task.FromResult(new UpdateAuthorResult
                    {
                        Success = false,
                        Message = "Author name already taken!"
                    });
                }
            }

            if (!string.IsNullOrEmpty(command.NewAuthorName))
                author.AuthorName = command.NewAuthorName;

            _authorRepository.Update(author);

            var authorData = _mapper.Map<UpdateAuthorResult.AuthorData>(author);

            return Task.FromResult(new UpdateAuthorResult
            {
                Success = true,
                Message = "Author updated successfully!",
                UpdatedAuthor = authorData
            });
        }
    }

    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, DeleteAuthorResult>
    {
        private readonly IAuthorRepository _authorRepository;

        public DeleteAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public Task<DeleteAuthorResult> Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
        {
            var author = _authorRepository.GetById(command.AuthorId);

            if (author == null)
            {
                return Task.FromResult(new DeleteAuthorResult
                {
                    Success = false,
                    Message = "Author not found!"
                });
            }

            _authorRepository.Delete(command.AuthorId);

            return Task.FromResult(new DeleteAuthorResult
            {
                Success = true,
                Message = "Author deleted successfully!"
            });
        }
    }

    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, GetAuthorResult>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public GetAuthorByIdQueryHandler(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public Task<GetAuthorResult> Handle(GetAuthorByIdQuery query, CancellationToken cancellationToken)
        {
            var author = _authorRepository.GetById(query.AuthorId);
            
            if (author == null)
            {
                return Task.FromResult(new GetAuthorResult
                {
                    Success = false,
                    Message = "Author not found!"
                });
            }

            var authorData = _mapper.Map<GetAuthorResult.AuthorData>(author);

            return Task.FromResult(new GetAuthorResult
            {
                Success = true,
                Message = "Author found",
                Author = authorData 
            });
        }
    }

    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, GetAllAuthorsResult>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public GetAllAuthorsQueryHandler(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public Task<GetAllAuthorsResult> Handle(GetAllAuthorsQuery query, CancellationToken cancellationToken)
        {
            var authors = _authorRepository.GetAll();

            var authorSummaries = _mapper.Map<List<GetAllAuthorsResult.AuthorSummary>>(authors);

            return Task.FromResult(new GetAllAuthorsResult
            {
                Success = true,
                Authors = authorSummaries  
            });
        }
    }
}