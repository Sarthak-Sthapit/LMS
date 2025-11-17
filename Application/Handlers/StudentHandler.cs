using RestAPI.Application.Commands;
using RestAPI.Application.Queries;
using RestAPI.Models;
using RestAPI.Repositories;
using MediatR;
using AutoMapper;

namespace RestAPI.Application.Handlers
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, CreateStudentResult>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public CreateStudentCommandHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public Task<CreateStudentResult> Handle(CreateStudentCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Name))
            {
                return Task.FromResult(new CreateStudentResult
                {
                    Success = false,
                    Message = "Student name is required"
                });
            }

            var existingStudent = _studentRepository.GetByName(command.Name);
            if (existingStudent != null)
            {
                return Task.FromResult(new CreateStudentResult
                {
                    Success = false,
                    Message = "Student already exists!"
                });
            }

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
            var studentData = _mapper.Map<CreateStudentResult.StudentData>(newStudent);

            return Task.FromResult(new CreateStudentResult
            {
                Success = true,
                StudentId = newStudent.StudentId,
                Message = "Student Created Successfully",
                Student = studentData
            });
        }
    }

    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, UpdateStudentResult>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public UpdateStudentCommandHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public Task<UpdateStudentResult> Handle(UpdateStudentCommand command, CancellationToken cancellationToken)
        {
            var student = _studentRepository.GetById(command.StudentId);
            if (student == null)
            {
                return Task.FromResult(new UpdateStudentResult
                {
                    Success = false,
                    Message = "Student not Found!"
                });
            }

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
            var studentData = _mapper.Map<UpdateStudentResult.StudentData>(student);

            return Task.FromResult(new UpdateStudentResult
            {
                Success = true,
                Message = "Student updated successfully!",
                UpdatedStudent = studentData
            });
        }
    }

    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, DeleteStudentResult>
    {
        private readonly IStudentRepository _studentRepository;

        public DeleteStudentCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public Task<DeleteStudentResult> Handle(DeleteStudentCommand command, CancellationToken cancellationToken)
        {
            var student = _studentRepository.GetById(command.StudentId);
            if (student == null)
            {
                return Task.FromResult(new DeleteStudentResult
                {
                    Success = false,
                    Message = "Student not found"
                });
            }

            _studentRepository.Delete(command.StudentId);

            return Task.FromResult(new DeleteStudentResult
            {
                Success = true,
                Message = "Student Deleted Successfully!"
            });
        }

    }
    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, GetStudentResult>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public GetStudentByIdQueryHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public Task<GetStudentResult> Handle(GetStudentByIdQuery query, CancellationToken cancellationToken)
        {
            var student = _studentRepository.GetById(query.StudentId);
            if(student == null)
            {
                return Task.FromResult(new GetStudentResult
                {
                    Success = false,
                    Message = "Student not found"
                });
            }

            var studentData = _mapper.Map<GetStudentResult.StudentData>(student);

            return Task.FromResult(new GetStudentResult
            {
                Success = true,
                Message = "Student found",
                Student = studentData
            });
        }
    }

    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, GetAllStudentsResult>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public GetAllStudentsQueryHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public Task<GetAllStudentsResult> Handle(GetAllStudentsQuery query, CancellationToken cancellationToken)
        {
            var students = _studentRepository.GetAll();

            var studentSummaries = _mapper.Map<List<GetAllStudentsResult.StudentSummary>>(students);

            return Task.FromResult(new GetAllStudentsResult
            {
                Success = true,
                Students = studentSummaries
            });
        }
    }
}