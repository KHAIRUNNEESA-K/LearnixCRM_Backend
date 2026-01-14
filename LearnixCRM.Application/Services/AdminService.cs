using AutoMapper;
using LearnixCRM.Application.DTOs;
using LearnixCRM.Application.Interfaces;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;

namespace LearnixCRM.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AdminService(
            IAdminRepository repository,
            IMapper mapper,
            IEmailService emailService)
        {
            _repository = repository;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<UserResponseDto> InviteUserAsync(CreateUserRequestDto dto, string adminName)
        {
           
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is required");
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException("FullName is required");
            if (string.IsNullOrWhiteSpace(dto.Role))
                throw new ArgumentException("Role is required");


            var existingUser = await _repository.GetAllUsersAsync();
            if (existingUser.Any(u => u.Email == dto.Email))
                throw new InvalidOperationException("Email already exists");


            if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
                throw new ArgumentException("Invalid role");


            var user = User.CreateInvitedUser(dto.Email, dto.FullName, role, adminName);

            if (user.UserRole == UserRole.Admin)
                throw new InvalidOperationException("Admin user cannot be deleted");

            await _repository.CreateUserAsync(user);
            var invite = new UserInvite(user.Email, adminName);
            await _repository.CreateInviteAsync(invite);


            var inviteLink = $"https://app.learnixcrm.com/accept-invite?token={invite.Token}";
            await _emailService.SendInviteAsync(user.Email, inviteLink);

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<IEnumerable<UserResponseDto>> GetPendingUsersAsync()
        {
            var users = await _repository.GetPendingUsersAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }
        public async Task ActivateUserAsync(int userId, string password, string adminName)
        {
            var user = await _repository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException("User not found");

            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            user.Activate(hash, adminName);

            await _repository.UpdateUserAsync(user);
        }

        public async Task DeactivateUserAsync(int userId, string adminName)
        {
            var user = await _repository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException("User not found");

            user.Deactivate(adminName);

            await _repository.UpdateUserAsync(user);
        }
        public async Task ChangeUserRoleAsync(int userId,string newRole,string adminName)
        {
            var user = await _repository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException("User not found");

            var role = Enum.Parse<UserRole>(newRole, true);
            user.ChangeRole(role, adminName);

            await _repository.UpdateUserAsync(user);
        }
        public async Task ResendInviteAsync(int userId, string adminName)
        {
            var user = await _repository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException("User not found");

            if (user.Status != UserStatus.Pending)
                throw new InvalidOperationException("User already active");

            var invite = await _repository.GetInviteByEmailAsync(user.Email);

            if (invite == null || invite.IsUsed || invite.ExpiryDate <= DateTime.UtcNow || invite.IsDeleted)
            {
                invite = new UserInvite(user.Email, adminName);
                await _repository.CreateInviteAsync(invite);
            }
            else
            {
                invite.ExtendExpiry(adminName, 2);
                await _repository.SaveInviteAsync(invite);
            }

            var link = $"https://app.learnixcrm.com/accept-invite?token={invite.Token}";
            await _emailService.SendInviteAsync(user.Email, link);
        }


        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _repository.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }
        public async Task ActivateUserAsync(int userId, string adminName)
        {
            var user = await _repository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException("User not found");

            user.Activate(
                passwordHash: user.PasswordHash!,
                updatedBy: adminName);

            await _repository.UpdateUserAsync(user);
        }
        public async Task DeleteUserAsync(int userId, string adminName)
        {
            var user = await _repository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException("User not found");

            if (user.UserRole == UserRole.Admin)
                throw new Exception("Admin user cannot be deleted");

            await _repository.DeleteUserAsync(userId, adminName);
        }
        public async Task<IEnumerable<UserInviteDto>> GetPendingInvitesAsync()
        {
            var invites = await _repository.GetPendingInvitesAsync();
            return _mapper.Map<IEnumerable<UserInviteDto>>(invites);
        }
    }
}
