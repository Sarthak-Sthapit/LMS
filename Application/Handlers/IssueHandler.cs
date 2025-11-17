using RestAPI.Application.Commands;
using RestAPI.Application.Queries;
using RestAPI.Models;
using RestAPI.Repositories;
using MediatR;
using AutoMapper;

namespace RestAPI.Application.Handlers
{
    public class CreateIssueCommandHandler : IRequestHandler<CreateIssueCommand, CreateIssueResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public CreateIssueCommandHandler(
            IIssueRepository issueRepository,
            IBookRepository bookRepository,
            IStudentRepository studentRepository,
            IMapper mapper)
        {
            _issueRepository = issueRepository;
            _bookRepository = bookRepository;
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public Task<CreateIssueResult> Handle(CreateIssueCommand command, CancellationToken cancellationToken)
        {
            var book = _bookRepository.GetById(command.BookId);
            if (book == null)
            {
                return Task.FromResult(new CreateIssueResult
                {
                    Success = false,
                    Message = "Book not found!"
                });
            }

            var student = _studentRepository.GetById(command.StudentId);
            if (student == null)
            {
                return Task.FromResult(new CreateIssueResult
                {
                    Success = false,
                    Message = "Student not found!"
                });
            }

            var newIssue = new Issue
            {
                BookId = command.BookId,
                StudentId = command.StudentId,
                IssueDate = DateTime.UtcNow,
                IsDeleted = false
            };

            _issueRepository.Add(newIssue);

            var issueWithData = _issueRepository.GetById(newIssue.IssueId);
            var issueData = _mapper.Map<CreateIssueResult.IssueData>(issueWithData);

            return Task.FromResult(new CreateIssueResult
            {
                Success = true,
                IssueId = newIssue.IssueId,
                Message = "Issue created successfully!",
                Issue = issueData
            });
        }
    }

    public class UpdateIssueCommandHandler : IRequestHandler<UpdateIssueCommand, UpdateIssueResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public UpdateIssueCommandHandler(
            IIssueRepository issueRepository,
            IBookRepository bookRepository,
            IStudentRepository studentRepository,
            IMapper mapper)
        {
            _issueRepository = issueRepository;
            _bookRepository = bookRepository;
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public Task<UpdateIssueResult> Handle(UpdateIssueCommand command, CancellationToken cancellationToken)
        {
            var issue = _issueRepository.GetById(command.IssueId);
            if (issue == null)
            {
                return Task.FromResult(new UpdateIssueResult
                {
                    Success = false,
                    Message = "Issue not found!"
                });
            }

            if (command.NewBookId.HasValue)
            {
                var book = _bookRepository.GetById(command.NewBookId.Value);
                if (book == null)
                {
                    return Task.FromResult(new UpdateIssueResult
                    {
                        Success = false,
                        Message = "Book not found!"
                    });
                }
                issue.BookId = command.NewBookId.Value;
            }

            if (command.NewStudentId.HasValue)
            {
                var student = _studentRepository.GetById(command.NewStudentId.Value);
                if (student == null)
                {
                    return Task.FromResult(new UpdateIssueResult
                    {
                        Success = false,
                        Message = "Student not found!"
                    });
                }
                issue.StudentId = command.NewStudentId.Value;
            }

            _issueRepository.Update(issue);

            var updatedIssue = _issueRepository.GetById(issue.IssueId);
            var issueData = _mapper.Map<UpdateIssueResult.IssueData>(updatedIssue);

            return Task.FromResult(new UpdateIssueResult
            {
                Success = true,
                Message = "Issue updated successfully!",
                UpdatedIssue = issueData
            });
        }
    }

    public class DeleteIssueCommandHandler : IRequestHandler<DeleteIssueCommand, DeleteIssueResult>
    {
        private readonly IIssueRepository _issueRepository;

        public DeleteIssueCommandHandler(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        public Task<DeleteIssueResult> Handle(DeleteIssueCommand command, CancellationToken cancellationToken)
        {
            var issue = _issueRepository.GetById(command.IssueId);

            if (issue == null)
            {
                return Task.FromResult(new DeleteIssueResult
                {
                    Success = false,
                    Message = "Issue not found!"
                });
            }

            _issueRepository.Delete(command.IssueId);

            return Task.FromResult(new DeleteIssueResult
            {
                Success = true,
                Message = "Issue deleted successfully!"
            });
        }
    }

    public class GetIssueByIdQueryHandler : IRequestHandler<GetIssueByIdQuery, GetIssueResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;

        public GetIssueByIdQueryHandler(IIssueRepository issueRepository, IMapper mapper)
        {
            _issueRepository = issueRepository;
            _mapper = mapper;
        }

        public Task<GetIssueResult> Handle(GetIssueByIdQuery query, CancellationToken cancellationToken)
        {
            var issue = _issueRepository.GetById(query.IssueId);
            
            if (issue == null)
            {
                return Task.FromResult(new GetIssueResult
                {
                    Success = false,
                    Message = "Issue not found!"
                });
            }

            var issueData = _mapper.Map<GetIssueResult.IssueData>(issue);

            return Task.FromResult(new GetIssueResult
            {
                Success = true,
                Message = "Issue found",
                Issue = issueData
            });
        }
    }

    public class GetAllIssuesQueryHandler : IRequestHandler<GetAllIssuesQuery, GetAllIssuesResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;

        public GetAllIssuesQueryHandler(IIssueRepository issueRepository, IMapper mapper)
        {
            _issueRepository = issueRepository;
            _mapper = mapper;
        }

        public Task<GetAllIssuesResult> Handle(GetAllIssuesQuery query, CancellationToken cancellationToken)
        {
            var issues = _issueRepository.GetAll();
            var issueSummaries = _mapper.Map<List<GetAllIssuesResult.IssueSummary>>(issues);

            return Task.FromResult(new GetAllIssuesResult
            {
                Success = true,
                Issues = issueSummaries
            });
        }
    }

    public class GetIssuesByStudentQueryHandler : IRequestHandler<GetIssuesByStudentQuery, GetAllIssuesResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;

        public GetIssuesByStudentQueryHandler(IIssueRepository issueRepository, IMapper mapper)
        {
            _issueRepository = issueRepository;
            _mapper = mapper;
        }

        public Task<GetAllIssuesResult> Handle(GetIssuesByStudentQuery query, CancellationToken cancellationToken)
        {
            var issues = _issueRepository.GetByStudentId(query.StudentId);
            var issueSummaries = _mapper.Map<List<GetAllIssuesResult.IssueSummary>>(issues);

            return Task.FromResult(new GetAllIssuesResult
            {
                Success = true,
                Issues = issueSummaries
            });
        }
    }

    public class GetIssuesByBookQueryHandler : IRequestHandler<GetIssuesByBookQuery, GetAllIssuesResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;

        public GetIssuesByBookQueryHandler(IIssueRepository issueRepository, IMapper mapper)
        {
            _issueRepository = issueRepository;
            _mapper = mapper;
        }

        public Task<GetAllIssuesResult> Handle(GetIssuesByBookQuery query, CancellationToken cancellationToken)
        {
            var issues = _issueRepository.GetByBookId(query.BookId);
            var issueSummaries = _mapper.Map<List<GetAllIssuesResult.IssueSummary>>(issues);

            return Task.FromResult(new GetAllIssuesResult
            {
                Success = true,
                Issues = issueSummaries
            });
        }
    }
}