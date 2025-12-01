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
    // checking out book
    public class CheckoutBookCommandHandler : IRequestHandler<CheckoutBookCommand, CheckoutBookResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public CheckoutBookCommandHandler(
            IIssueRepository issueRepository,
            IBookRepository bookRepository,
            IStudentRepository studentRepository,
            IMapper mapper,
            ILoggingService logger)
        {
            _issueRepository = issueRepository;
            _bookRepository = bookRepository;
            _studentRepository = studentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<CheckoutBookResult> Handle(CheckoutBookCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Checking out book: BookId={BookId}, StudentId={StudentId}", 
                    command.BookId, command.StudentId);

                // Validate book exists
                var book = _bookRepository.GetById(command.BookId);
                if (book == null)
                {
                    throw new NotFoundException("Book", command.BookId);
                }

                // Check availability
                if (book.AvailableCopies <= 0)
                {
                    throw new BusinessRuleException("Book is not available. All copies are currently issued.");
                }

                // Validate student exists
                var student = _studentRepository.GetById(command.StudentId);
                if (student == null)
                {
                    throw new NotFoundException("Student", command.StudentId);
                }

                // Check if student already has this book
                var existingIssue = _issueRepository.GetByStudentId(command.StudentId)
                    .FirstOrDefault(i => i.BookId == command.BookId && !i.IsReturned);
                
                if (existingIssue != null)
                {
                    throw new BusinessRuleException("Student already has this book checked out");
                }

                // Create new issue
                var issueDate = DateTime.UtcNow;
                var dueDate = issueDate.AddDays(command.BorrowDays);

                var newIssue = new Issue
                {
                    BookId = command.BookId,
                    StudentId = command.StudentId,
                    IssueDate = issueDate,
                    DueDate = dueDate,
                    IsReturned = false,
                    IsDeleted = false
                };

                _issueRepository.Add(newIssue);

                // Decrease available copies
                book.AvailableCopies--;
                _bookRepository.Update(book);

                _logger.LogInformation("Book checked out successfully: IssueId={IssueId}", newIssue.IssueId);

                var issueWithData = _issueRepository.GetById(newIssue.IssueId);
                var issueData = _mapper.Map<CheckoutBookResult.IssueData>(issueWithData);

                return Task.FromResult(new CheckoutBookResult
                {
                    Success = true,
                    Message = $"Book checked out successfully! Due date: {dueDate:MMM dd, yyyy}",
                    Issue = issueData
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error checking out book: BookId={BookId}", command.BookId);
                throw;
            }
        }
    }

    // returning book
    public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, ReturnBookResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public ReturnBookCommandHandler(
            IIssueRepository issueRepository,
            IBookRepository bookRepository,
            IMapper mapper,
            ILoggingService logger)
        {
            _issueRepository = issueRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<ReturnBookResult> Handle(ReturnBookCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Returning book: IssueId={IssueId}", command.IssueId);

                var issue = _issueRepository.GetById(command.IssueId);
                
                if (issue == null)
                {
                    throw new NotFoundException("Issue record", command.IssueId);
                }

                if (issue.IsReturned)
                {
                    throw new BusinessRuleException("Book has already been returned");
                }

                // Mark as returned
                var returnDate = DateTime.UtcNow;
                issue.ReturnDate = returnDate;
                issue.IsReturned = true;

                // Calculate overdue days
                var daysOverdue = issue.IsOverdue ? (returnDate - issue.DueDate).Days : 0;

                _issueRepository.Update(issue);

                // Increase available copies
                var book = _bookRepository.GetById(issue.BookId);
                if (book != null)
                {
                    book.AvailableCopies++;
                    _bookRepository.Update(book);
                }

                _logger.LogInformation("Book returned successfully: IssueId={IssueId}, DaysOverdue={DaysOverdue}", 
                    command.IssueId, daysOverdue);

                var issueData = _mapper.Map<ReturnBookResult.IssueData>(issue);

                var message = daysOverdue > 0
                    ? $"Book returned successfully! {daysOverdue} days overdue."
                    : "Book returned successfully!";

                return Task.FromResult(new ReturnBookResult
                {
                    Success = true,
                    Message = message,
                    DaysOverdue = daysOverdue,
                    Issue = issueData
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error returning book: IssueId={IssueId}", command.IssueId);
                throw;
            }
        }
    }

    // getting active loans
    public class GetActiveLoansQueryHandler : IRequestHandler<GetActiveLoansQuery, GetActiveLoansResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public GetActiveLoansQueryHandler(
            IIssueRepository issueRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _issueRepository = issueRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<GetActiveLoansResult> Handle(GetActiveLoansQuery query, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching active loans");

                var activeIssues = _issueRepository.GetAll()
                    .Where(i => !i.IsReturned)
                    .OrderByDescending(i => i.IssueDate)
                    .ToList();

                var loanSummaries = _mapper.Map<List<GetActiveLoansResult.LoanSummary>>(activeIssues);

                _logger.LogInformation("Retrieved {Count} active loans", activeIssues.Count);

                return Task.FromResult(new GetActiveLoansResult
                {
                    Success = true,
                    Loans = loanSummaries
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching active loans");
                throw;
            }
        }
    }

    // getting overdue books
    public class GetOverdueBooksQueryHandler : IRequestHandler<GetOverdueBooksQuery, GetActiveLoansResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public GetOverdueBooksQueryHandler(
            IIssueRepository issueRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _issueRepository = issueRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<GetActiveLoansResult> Handle(GetOverdueBooksQuery query, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching overdue books");

                var overdueIssues = _issueRepository.GetAll()
                    .Where(i => !i.IsReturned && i.IsOverdue)
                    .OrderByDescending(i => i.DaysOverdue)
                    .ToList();

                var loanSummaries = _mapper.Map<List<GetActiveLoansResult.LoanSummary>>(overdueIssues);

                _logger.LogInformation("Retrieved {Count} overdue books", overdueIssues.Count);

                return Task.FromResult(new GetActiveLoansResult
                {
                    Success = true,
                    Loans = loanSummaries
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching overdue books");
                throw;
            }
        }
    }


    public class CreateIssueCommandHandler : IRequestHandler<CreateIssueCommand, CreateIssueResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public CreateIssueCommandHandler(
            IIssueRepository issueRepository,
            IBookRepository bookRepository,
            IStudentRepository studentRepository,
            IMapper mapper,
            ILoggingService logger)
        {
            _issueRepository = issueRepository;
            _bookRepository = bookRepository;
            _studentRepository = studentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<CreateIssueResult> Handle(CreateIssueCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var book = _bookRepository.GetById(command.BookId);
                if (book == null)
                {
                    throw new NotFoundException("Book", command.BookId);
                }

                var student = _studentRepository.GetById(command.StudentId);
                if (student == null)
                {
                    throw new NotFoundException("Student", command.StudentId);
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
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error creating issue");
                throw;
            }
        }
    }


    public class DeleteIssueCommandHandler : IRequestHandler<DeleteIssueCommand, DeleteIssueResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly ILoggingService _logger;

        public DeleteIssueCommandHandler(
            IIssueRepository issueRepository,
            ILoggingService logger)
        {
            _issueRepository = issueRepository;
            _logger = logger;
        }

        public Task<DeleteIssueResult> Handle(DeleteIssueCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var issue = _issueRepository.GetById(command.IssueId);
                if (issue == null)
                {
                    throw new NotFoundException("Issue", command.IssueId);
                }

                _issueRepository.Delete(command.IssueId);

                return Task.FromResult(new DeleteIssueResult
                {
                    Success = true,
                    Message = "Issue deleted successfully!"
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error deleting issue: {IssueId}", command.IssueId);
                throw;
            }
        }
    }


    public class GetAllIssuesQueryHandler : IRequestHandler<GetAllIssuesQuery, GetAllIssuesResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public GetAllIssuesQueryHandler(
            IIssueRepository issueRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _issueRepository = issueRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<GetAllIssuesResult> Handle(GetAllIssuesQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var issues = _issueRepository.GetAll();
                var issueSummaries = _mapper.Map<List<GetAllIssuesResult.IssueSummary>>(issues);

                return Task.FromResult(new GetAllIssuesResult
                {
                    Success = true,
                    Issues = issueSummaries
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching all issues");
                throw;
            }
        }
    }

    public class GetIssueByIdQueryHandler : IRequestHandler<GetIssueByIdQuery, GetIssueResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public GetIssueByIdQueryHandler(
            IIssueRepository issueRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _issueRepository = issueRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<GetIssueResult> Handle(GetIssueByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var issue = _issueRepository.GetById(query.IssueId);
                
                if (issue == null)
                {
                    throw new NotFoundException("Issue", query.IssueId);
                }

                var issueData = _mapper.Map<GetIssueResult.IssueData>(issue);

                return Task.FromResult(new GetIssueResult
                {
                    Success = true,
                    Message = "Issue found",
                    Issue = issueData
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error getting issue: {IssueId}", query.IssueId);
                throw;
            }
        }
    }

  
    public class GetIssuesByStudentQueryHandler : IRequestHandler<GetIssuesByStudentQuery, GetAllIssuesResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public GetIssuesByStudentQueryHandler(
            IIssueRepository issueRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _issueRepository = issueRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<GetAllIssuesResult> Handle(GetIssuesByStudentQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var issues = _issueRepository.GetByStudentId(query.StudentId);
                var issueSummaries = _mapper.Map<List<GetAllIssuesResult.IssueSummary>>(issues);

                return Task.FromResult(new GetAllIssuesResult
                {
                    Success = true,
                    Issues = issueSummaries
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching issues for student: {StudentId}", query.StudentId);
                throw;
            }
        }
    }

    
    public class GetIssuesByBookQueryHandler : IRequestHandler<GetIssuesByBookQuery, GetAllIssuesResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public GetIssuesByBookQueryHandler(
            IIssueRepository issueRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _issueRepository = issueRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<GetAllIssuesResult> Handle(GetIssuesByBookQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var issues = _issueRepository.GetByBookId(query.BookId);
                var issueSummaries = _mapper.Map<List<GetAllIssuesResult.IssueSummary>>(issues);

                return Task.FromResult(new GetAllIssuesResult
                {
                    Success = true,
                    Issues = issueSummaries
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching issues for book: {BookId}", query.BookId);
                throw;
            }
        }
    }

    
}