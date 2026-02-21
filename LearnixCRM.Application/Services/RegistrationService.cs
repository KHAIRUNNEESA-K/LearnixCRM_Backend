using AutoMapper;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;

public class RegistrationService : IRegistrationService
{
    private readonly IRegistrationRepository _repository;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;

    public RegistrationService(
        IRegistrationRepository repository,
        IEmailService emailService,
        IMapper mapper)
    {
        _repository = repository;
        _emailService = emailService;
        _mapper = mapper;
    }
    public async Task<RegisterUserResponseDto> RegisterAsync(RegisterUserRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FirstName))
            throw new ArgumentException("First name is required");

        if (string.IsNullOrWhiteSpace(dto.LastName))
            throw new ArgumentException("Last name is required");

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException("Email is required");

        dto.FirstName = dto.FirstName.Trim();
        dto.LastName = dto.LastName.Trim();
        dto.Email = dto.Email.Trim().ToLowerInvariant();
        dto.ContactNumber = string.IsNullOrWhiteSpace(dto.ContactNumber) ? null : dto.ContactNumber.Trim();

        var role = (UserRole)dto.Role;

        if (!Enum.IsDefined(typeof(UserRole), role))
            throw new ArgumentException("Invalid role");

        if (role != UserRole.Sales && role != UserRole.Manager)
            throw new InvalidOperationException("Only Sales and Manager can self-register.");


        var exists = await _repository.IsEmailRegisteredAsync(dto.Email);
        if (exists)
            throw new InvalidOperationException("Email already registered");

        var user = User.CreateSelfRegisteredUser(
         dto.Email,
         dto.FirstName,
         dto.LastName,
         dto.ContactNumber,
         role
     );

        await _repository.CreateUserAsync(user);

        user.SetCreatedBy(user.UserId);

        await _repository.UpdateAsync(user); 

        await _emailService.SendUserRegisteredAsync(dto.Email);

        return _mapper.Map<RegisterUserResponseDto>(user);
    }

}

