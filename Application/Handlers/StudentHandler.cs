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
    
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, CreateStudentResult>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public CreateStudentCommandHandler(
            IStudentRepository studentRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<CreateStudentResult> Handle(CreateStudentCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating student: {Name}", command.Name);

                // Validation
                if (string.IsNullOrEmpty(command.Name))
                {
                    throw new ValidationException("Name", "Student name is required");
                }

                // Check for duplicate
                var existingStudent = _studentRepository.GetByName(command.Name);
                if (existingStudent != null)
                {
                    throw new ConflictException($"Student '{command.Name}' already exists");
                }

                // Create new student
                var newStudent = new Student
                {
                    Name = command.Name,
                    Address = command.Address,
                    ContactNo = command.ContactNo,
                    Faculty = command.Faculty,
                    Semester = command.Semester,
                    IsDeleted = false
                };

                _studentRepository.Add(newStudent);

                _logger.LogInformation("Student created successfully: {StudentId}", newStudent.StudentId);

                var studentData = _mapper.Map<CreateStudentResult.StudentData>(newStudent);

                return Task.FromResult(new CreateStudentResult
                {
                    Success = true,
                    StudentId = newStudent.StudentId,
                    Message = "Student Created Successfully",
                    Student = studentData
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error creating student: {Name}", command.Name);
                throw;
            }
        }
    }

    // update student
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, UpdateStudentResult>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public UpdateStudentCommandHandler(
            IStudentRepository studentRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<UpdateStudentResult> Handle(UpdateStudentCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating student: {StudentId}", command.StudentId);

                // Check if student exists
                var student = _studentRepository.GetById(command.StudentId);
                if (student == null)
                {
                    throw new NotFoundException("Student", command.StudentId);
                }

                // Update fields
                if (!string.IsNullOrEmpty(command.NewName))
                    student.Name = command.NewName;

                if (!string.IsNullOrEmpty(command.NewAddress))
                    student.Address = command.NewAddress;

                if (!string.IsNullOrEmpty(command.NewContactNo))
                    student.ContactNo = command.NewContactNo;

                if (!string.IsNullOrEmpty(command.NewFaculty))
                    student.Faculty = command.NewFaculty;

                if (!string.IsNullOrEmpty(command.NewSemester))
                    student.Semester = command.NewSemester;

                _studentRepository.Update(student);

                _logger.LogInformation("Student updated successfully: {StudentId}", command.StudentId);

                var studentData = _mapper.Map<UpdateStudentResult.StudentData>(student);

                return Task.FromResult(new UpdateStudentResult
                {
                    Success = true,
                    Message = "Student updated successfully!",
                    UpdatedStudent = studentData
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error updating student: {StudentId}", command.StudentId);
                throw;
            }
        }
    }

    // delete student
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, DeleteStudentResult>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILoggingService _logger;

        public DeleteStudentCommandHandler(
            IStudentRepository studentRepository,
            ILoggingService logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        public Task<DeleteStudentResult> Handle(DeleteStudentCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Deleting student: {StudentId}", command.StudentId);

                var student = _studentRepository.GetById(command.StudentId);
                if (student == null)
                {
                    throw new NotFoundException("Student", command.StudentId);
                }

                _studentRepository.Delete(command.StudentId);

                _logger.LogInformation("Student deleted successfully: {StudentId}", command.StudentId);

                return Task.FromResult(new DeleteStudentResult
                {
                    Success = true,
                    Message = "Student Deleted Successfully!"
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error deleting student: {StudentId}", command.StudentId);
                throw;
            }
        }
    }

    // get student by id
    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, GetStudentResult>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public GetStudentByIdQueryHandler(
            IStudentRepository studentRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<GetStudentResult> Handle(GetStudentByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var student = _studentRepository.GetById(query.StudentId);
                if (student == null)
                {
                    throw new NotFoundException("Student", query.StudentId);
                }

                var studentData = _mapper.Map<GetStudentResult.StudentData>(student);

                return Task.FromResult(new GetStudentResult
                {
                    Success = true,
                    Message = "Student found",
                    Student = studentData
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error getting student: {StudentId}", query.StudentId);
                throw;
            }
        }
    }

    // get all students
    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, GetAllStudentsResult>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public GetAllStudentsQueryHandler(
            IStudentRepository studentRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<GetAllStudentsResult> Handle(GetAllStudentsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching all students");

                var students = _studentRepository.GetAll();
                var studentSummaries = _mapper.Map<List<GetAllStudentsResult.StudentSummary>>(students);

                _logger.LogInformation("Retrieved {Count} students", students.Count);

                return Task.FromResult(new GetAllStudentsResult
                {
                    Success = true,
                    Students = studentSummaries
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching all students");
                throw;
            }
        }
    }
}