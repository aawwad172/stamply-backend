

using Stamply.Application.CQRS.Commands.Authentication;
using Stamply.Domain.Exceptions;
using Stamply.Domain.Interfaces.Application.Services;
using Stamply.Domain.Interfaces.Infrastructure.IRepositories;

using Microsoft.Extensions.Logging;
using Stamply.Domain.Entities.Identity;
using Stamply.Domain.Entities.Identity.Authentication;
using Stamply.Domain.ValueObjects;

namespace Stamply.Application.CQRS.CommandHandlers.Authentication;

public class RegisterUserCommandHandler(
    ICurrentUserService currentUserService,
    ILogger<RegisterUserCommandHandler> logger,
    IAuthenticationRepository authenticationRepository,
    IUserRepository userRepository,
    ISecurityService securityService,
    IRoleRepository roleRepository,
    IRepository<UserCredentials> userCredentialsRepository,
    IUnitOfWork unitOfWork) : BaseHandler<RegisterUserCommand, RegisterUserCommandResult>(currentUserService, logger, unitOfWork)
{
    private readonly IAuthenticationRepository _authenticationRepository = authenticationRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRepository<UserCredentials> _userCredentialsRepository = userCredentialsRepository;
    private readonly ISecurityService _securityService = securityService;
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly string _defaultRoleName = "User";

    public override async Task<RegisterUserCommandResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Check if a user already exists with the same email.
        User? existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
        if (existingUser is not null)
            throw new ConflictException("A user with this email already exists.");

        // Check if a user already exists with the same username.
        User? existingUsername = await _userRepository.GetUserByUsernameAsync(request.Username);
        if (existingUsername is not null)
            throw new ConflictException("A user with this username already exists.");

        // Hash the password using the security service (BCrypt)
        string hashedPassword = _securityService.HashSecret(request.Password);



        // Generate a new security stamp
        string securityStamp = Guid.NewGuid().ToString();

        Guid id = Guid.CreateVersion7();

        UserCredentials userCreds = new()
        {
            Id = Guid.CreateVersion7(),
            UserId = id,
            PasswordHash = hashedPassword

        };

        Role? defaultRole = await _roleRepository.GetRoleByNameAsync(_defaultRoleName);

        if (defaultRole is null)
        {
            // IMPORTANT: This prevents users from being created without a role if the DB isn't seeded.
            throw new InvalidOperationException($"The default role '{_defaultRoleName}' does not exist in the database. Please seed roles.");
        }

        User user = new()
        {
            Id = id,
            FullName = new FullName
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName
            },
            Email = request.Email,
            Username = request.Username,
            IsActive = false,
            IsDeleted = false,
            IsVerified = false,
            SecurityStamp = securityStamp,
        };

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _userRepository.AddAsync(user);
            await _userCredentialsRepository.AddAsync(userCreds);

            UserRole userRole = new()
            {
                UserId = user.Id,
                RoleId = defaultRole.Id,
            };
            await _authenticationRepository.AddUserRoleAsync(userRole); // Assuming IAuthRepository has this method

            await _unitOfWork.SaveAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return new RegisterUserCommandResult(
                Id: user.Id,
                FullName: user.FullName,
                Email: user.Email,
                Username: user.Username,
                IsActive: user.IsActive,
                IsVerified: user.IsVerified,
                Message: "User registered successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred during registration: {Message}", ex.Message);
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
