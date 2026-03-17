using AutoMapper;
using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;

namespace LearnixCRM.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ISetPasswordRepository _tokenRepository;
        private readonly IAssignUsersRepository _assignmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;
        public AdminService(
            IAdminRepository repository,
            IMapper mapper,
            IEmailService emailService,
            ISetPasswordRepository tokenRepository,
            IAssignUsersRepository assignmentRepository,
            IUserRepository userRepository,
            ITeamRepository teamRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _emailService = emailService;
            _tokenRepository=tokenRepository;
            _assignmentRepository=assignmentRepository;
            _userRepository=userRepository;
            _teamRepository=teamRepository;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _repository.GetAllUsersAsync();
            if (!users.Any())
            {
                throw new KeyNotFoundException("No users Found");
            }
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }
        public async Task<IEnumerable<UserResponseDto>> GetActiveUsersAsync()
        {
            var users = await _repository.GetActiveUsersAsync();

            if (!users.Any())
                throw new KeyNotFoundException("No active users found");

            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }
        public async Task<IEnumerable<UserResponseDto>> GetInactiveUsersAsync()
        { 
            var users = await _repository.GetInactiveUserAsync();

            if (!users.Any())
                throw new KeyNotFoundException("No Inactive users found");

            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }
        public async Task<IEnumerable<RejectResponseDto>> GetRejectedUsersAsync()
        {
            var users = await _repository.GetRejectedUserAsync();

            if (!users.Any())
                throw new KeyNotFoundException("No Rejected users found");

            return _mapper.Map<IEnumerable<RejectResponseDto>>(users);
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int userId)
        {
            var user = await _repository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found");
            }

             return _mapper.Map<UserResponseDto>(user);
        }
        public async Task<UserResponseDto> GetActiveUserByIdAsync(int userId)
        {
            var user = await _repository.GetActiveUserByIdAsync(userId);

            if (user == null)
                throw new KeyNotFoundException("Active user not found");

            return _mapper.Map<UserResponseDto>(user);
        }


        public async Task<IEnumerable<RegisterUserResponseDto>> GetPendingUsersAsync()
        {
            var users = await _repository.GetPendingUsersAsync();

            if (!users.Any())
            {
                throw new KeyNotFoundException("No pending users found");
            }

            return _mapper.Map<IEnumerable<RegisterUserResponseDto>>(users);
        }
        public async Task<IEnumerable<RegisterUserResponseDto>> GetApproveUsersAsync()
        {
            var users = await _repository.GetApproveUserAsync();

            if (!users.Any())
            {
                throw new KeyNotFoundException("No Approve users found");
            }

            return _mapper.Map<IEnumerable<RegisterUserResponseDto>>(users);
        }
        public async Task<IEnumerable<UserResponseDto>> GetBlockedUsersAsync()
        {
            var users = await _repository.GetBlockedUserAsync();

            if (!users.Any())
            {
                throw new KeyNotFoundException("No blocked users found");
            }

            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }


        public async Task<RegisterUserResponseDto> ApproveUserAndSendTokenAsync(int userId, int adminId)
        {
            var user = await _repository.GetUserByIdAsync(userId)
                ?? throw new KeyNotFoundException("User not found");

            if (user == null)
                throw new KeyNotFoundException("User not found for approval.");
            if (user.Status != UserStatus.Pending)
                throw new InvalidOperationException("Only pending users can be approved.");

            user.MarkApproved(adminId);

            if (user.UserRole == UserRole.Manager || user.UserRole == UserRole.Sales)
            {
                var employeeCode = $"EMP{user.UserId:D4}";
                user.AssignEmployeeCode(employeeCode, adminId);
            }

            await _repository.UpdateUserAsync(user);


            await _tokenRepository.InvalidateExistingTokensAsync(
                user.UserId,
                PasswordTokenType.SetPassword,
                adminId);

            var token = UserPasswordToken.Create(user.UserId, PasswordTokenType.SetPassword, adminId);
            Console.WriteLine($"Raw token for user {user.Email}: {token.RawToken}");
            await _tokenRepository.CreateAsync(token);

            var link = $"https://yourfrontend.com/set-password?token={token.RawToken}";
            await _emailService.SendApprovalEmailAsync(user.Email, link);

            return _mapper.Map<RegisterUserResponseDto>(user);
        }


        public async Task<RejectResponseDto> RejectUserAsync( int userId,int adminId, string rejectReason)
        {
            var user = await _repository.GetUserByIdAsync(userId);
            if(user ==null)
            {
                throw new KeyNotFoundException("User not found for Reject");
            }
            if (user.Status != UserStatus.Pending)
                throw new InvalidOperationException("Only pending users can be rejected.");
            if(user.Status == UserStatus.Rejected)
            {
                throw new InvalidOperationException("User is already  rejected.");
            }

            user.Reject(adminId, rejectReason);

            await _repository.UpdateUserAsync(user);

            await _emailService.SendRejectionEmailAsync(user.Email, rejectReason);

            return _mapper.Map<RejectResponseDto>(user);
        }

        public async Task<UserResponseDto> BlockUserAsync(int userId, int adminId)
        {
            var user = await _repository.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            if (user.UserRole == UserRole.Manager)
            {
                var hasTeam = await _teamRepository.ManagerHasActiveTeamAsync(userId);

                if (hasTeam)
                    throw new InvalidOperationException(
                        "Cannot block manager. Reassign or remove sales team first.");
            }
            if (user.Status==UserStatus.Blocked)
            {
                throw new InvalidOperationException("User is already blocked");
            }

            user.Block(adminId);

            await _repository.UpdateUserAsync(user);
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task UnblockUserAsync(int userId, int adminId)
        {
            var user = await _repository.GetUserByIdAsync(userId)
                ?? throw new KeyNotFoundException($"User with ID {userId} not found");

            user.Unblock(adminId);

            await _repository.UpdateUserAsync(user);
        }


        public async Task DeleteUserAsync(int userId, int adminId)
        {
            var user = await _repository.GetUserByIdAsync(userId);

            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");

            if (user.UserRole == UserRole.Admin)
                throw new InvalidOperationException("Admin user cannot be deleted");

            if (user.UserRole == UserRole.Manager)
            {
                var hasActiveTeam =
                    await _teamRepository.ManagerHasActiveTeamAsync(userId);

                if (hasActiveTeam)
                    throw new InvalidOperationException(
                        "Manager has an active team. Reassign the team before deleting this manager.");
            }

            await _repository.DeleteUserAsync(userId, adminId);
        }
        public async Task<IEnumerable<UserResponseDto>> GetActiveManagersAsync()
        {
            var managers = await _repository.GetUsersByRoleAndStatusAsync(
                UserRole.Manager,
                UserStatus.Active);
            if (!managers.Any())
            {
                throw new KeyNotFoundException("No active Managers found");
            }
            return _mapper.Map<IEnumerable<UserResponseDto>>(managers);

        }

        public async Task<IEnumerable<UserResponseDto>> GetActiveSalesExecutivesAsync()
        {
            var sales = await _repository.GetUsersByRoleAndStatusAsync(
                UserRole.Sales,
                UserStatus.Active);
            if (!sales.Any())
            {
                throw new KeyNotFoundException("No active Sales found");
            }


            return _mapper.Map<IEnumerable<UserResponseDto>>(sales);
        }
        public async Task ResendSetPasswordTokenAsync(string email, int adminId)
        {
            var user = await _userRepository.GetByEmailAsync(email)
                ?? throw new KeyNotFoundException("User not found");

            if (user.PasswordHash != null)
                throw new InvalidOperationException("User already set password");

            if (user.Status != UserStatus.Approved)
                throw new InvalidOperationException("User not eligible for set password");

            await _tokenRepository.InvalidateExistingTokensAsync(
                user.UserId,
                PasswordTokenType.SetPassword,
                adminId);  

            var newToken = UserPasswordToken.Create(
                user.UserId,
                PasswordTokenType.SetPassword,
                adminId);

            Console.WriteLine($"SET PASSWORD RAW TOKEN: {newToken.RawToken}");

            await _tokenRepository.CreateAsync(newToken);


            var link = $"https://yourfrontend.com/set-password?token={newToken.RawToken}";

            await _emailService.SendApprovalEmailAsync(user.Email, link);
        }

        public async Task<IEnumerable<UserResponseDto>> GetApprovedUsersPendingPasswordAsync()
        {
            var users = await _repository.GetApprovedUsersPendingPasswordAsync();

            if (!users.Any())
                throw new KeyNotFoundException("No approved users pending password setup");

            return users.Select(u => new UserResponseDto
            {
                UserId = u.UserId,
                FullName = u.FullName,
                Email = u.Email,
                Role = (int)u.UserRole,
                Status = (int)u.Status
            });
        }

    }
}
