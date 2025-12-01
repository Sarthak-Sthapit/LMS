using RestAPI.Application.Commands;
using RestAPI.Application.Queries;
using RestAPI.Models;
using RestAPI.Repositories;
using RestAPI.Services;
using RestAPI.Exceptions;
using MediatR;
using AutoMapper;

namespace RestAPI.Application.Handlers
{
    // signup
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public CreateUserCommandHandler(
            IUserRepository userRepository, 
            JwtService jwtService, 
            IMapper mapper,
            ILoggingService logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating user: {Username}", command.Username);

                // Validation
                if (string.IsNullOrEmpty(command.Username) || string.IsNullOrEmpty(command.Password))
                {
                    var errors = new Dictionary<string, string[]>();
                    if (string.IsNullOrEmpty(command.Username))
                        errors["Username"] = new[] { "Username is required" };
                    if (string.IsNullOrEmpty(command.Password))
                        errors["Password"] = new[] { "Password is required" };
                    
                    throw new ValidationException("Username and Password are required", errors);
                }

                // Check for duplicate
                var existingUser = _userRepository.GetByUsername(command.Username);
                if (existingUser != null)
                {
                    throw new ConflictException($"User '{command.Username}' already exists");
                }

                // Create user
                var newUser = new User
                {
                    Username = command.Username,
                    Password = command.Password,
                    CreatedAt = DateTime.UtcNow
                };

                _userRepository.Add(newUser);

                var token = _jwtService.CreateToken(newUser);
                var userData = _mapper.Map<CreateUserResult.UserData>(newUser);

                _logger.LogInformation("User created successfully: {UserId}", newUser.Id);

                return Task.FromResult(new CreateUserResult
                {
                    Success = true,
                    UserId = newUser.Id,
                    Message = "User Created Successfully!",
                    Token = token,
                    User = userData
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error creating user: {Username}", command.Username);
                throw;
            }
        }
    }

    // update user
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public UpdateUserCommandHandler(
            IUserRepository userRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating user: {UserId}", command.UserId);

                // Check if user exists
                var user = _userRepository.GetById(command.UserId);
                if (user == null)
                {
                    throw new NotFoundException("User", command.UserId);
                }

                // Check for duplicate username
                if (!string.IsNullOrEmpty(command.NewUsername) && command.NewUsername != user.Username)
                {
                    var existingUser = _userRepository.GetByUsername(command.NewUsername);
                    if (existingUser != null)
                    {
                        throw new ConflictException($"Username '{command.NewUsername}' is already taken");
                    }
                }

                // Update
                if (!string.IsNullOrEmpty(command.NewUsername))
                    user.Username = command.NewUsername;
                
                if (!string.IsNullOrEmpty(command.NewPassword))
                    user.Password = command.NewPassword;

                _userRepository.Update(user);

                _logger.LogInformation("User updated successfully: {UserId}", command.UserId);

                var userData = _mapper.Map<UpdateUserResult.UserData>(user);

                return Task.FromResult(new UpdateUserResult
                {
                    Success = true,
                    Message = "User updated successfully!",
                    UpdatedUser = userData
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error updating user: {UserId}", command.UserId);
                throw;
            }
        }
    }

    // get user by id
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public GetUserByIdQueryHandler(
            IUserRepository userRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<GetUserResult> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var user = _userRepository.GetById(query.UserId);
                
                if (user == null)
                {
                    throw new NotFoundException("User", query.UserId);
                }

                var userData = _mapper.Map<GetUserResult.UserData>(user);

                return Task.FromResult(new GetUserResult
                {
                    Success = true,
                    Message = "User found",
                    User = userData
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error getting user: {UserId}", query.UserId);
                throw;
            }
        }
    }

    // get all users
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, GetAllUsersResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public GetAllUsersQueryHandler(
            IUserRepository userRepository, 
            IMapper mapper,
            ILoggingService logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<GetAllUsersResult> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching all users");

                var users = _userRepository.GetAll();
                var userSummaries = _mapper.Map<List<GetAllUsersResult.UserSummary>>(users);

                _logger.LogInformation("Retrieved {Count} users", users.Count);

                return Task.FromResult(new GetAllUsersResult
                {
                    Success = true,
                    Users = userSummaries
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching all users");
                throw;
            }
        }
    }

    // login
    public class AuthenticateUserQueryHandler : IRequestHandler<AuthenticateUserQuery, AuthenticateUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public AuthenticateUserQueryHandler(
            IUserRepository userRepository, 
            JwtService jwtService, 
            IMapper mapper,
            ILoggingService logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<AuthenticateUserResult> Handle(AuthenticateUserQuery query, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Authenticating user: {Username}", query.Username);

                // Validation
                if (string.IsNullOrEmpty(query.Username) || string.IsNullOrEmpty(query.Password))
                {
                    throw new ValidationException("Username and password are required");
                }

                // Find user
                var user = _userRepository.GetByUsername(query.Username);
                if (user == null || user.Password != query.Password)
                {
                    throw new UnauthorizedException("Invalid username or password");
                }

                var token = _jwtService.CreateToken(user);
                var userInfo = _mapper.Map<AuthenticateUserResult.UserInfo>(user);

                _logger.LogInformation("User authenticated successfully: {UserId}", user.Id);

                return Task.FromResult(new AuthenticateUserResult
                {
                    Success = true,
                    Message = "Authentication successful!",
                    Token = token,
                    User = userInfo
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error authenticating user: {Username}", query.Username);
                throw;
            }
        }
    }

    // =deleting uuser
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoggingService _logger;

        public DeleteUserCommandHandler(
            IUserRepository userRepository,
            ILoggingService logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public Task<DeleteUserResult> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Deleting user: {UserId}", command.UserId);

                var user = _userRepository.GetById(command.UserId);
                if (user == null)
                {
                    throw new NotFoundException("User", command.UserId);
                }

                _userRepository.Delete(command.UserId);

                _logger.LogInformation("User deleted successfully: {UserId}", command.UserId);

                return Task.FromResult(new DeleteUserResult
                {
                    Success = true,
                    Message = "User deleted successfully!"
                });
            }
            catch (Exception ex) when (ex is not AppException)
            {
                _logger.LogError(ex, "Unexpected error deleting user: {UserId}", command.UserId);
                throw;
            }
        }
    }
}