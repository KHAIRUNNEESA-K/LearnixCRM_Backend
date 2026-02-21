using LearnixCRM.Application.DTOs.AssignUsers;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;

namespace LearnixCRM.Application.Services
{
    public class AssignedSalesService : IAssignedSalesService
    {
        private readonly IAssignedSalesRepository _repository;

        public AssignedSalesService(IAssignedSalesRepository repository)
        {
            _repository = repository;
        }
        public async Task<ManagerWithSalesResponseDto> GetAllAssignedSalesAsync(int managerUserId)
        {
            if (managerUserId <= 0)
                throw new ArgumentException("Invalid manager id");

            var salesUsers = (await _repository.GetSalesByManagerAsync(managerUserId)).ToList();

            if (!salesUsers.Any())
                throw new KeyNotFoundException("No sales users assigned to this manager");

            return new ManagerWithSalesResponseDto
            {
                ManagerUserId = managerUserId,
                SalesUsers = salesUsers
            };
        }

        public async Task<SalesUserDto> GetAssignedSalesByIdAsync(int managerUserId, int salesUserId)
        {
            if (managerUserId <= 0 || salesUserId <= 0)
                throw new ArgumentException("Invalid manager or sales user id");

            var salesUser = await _repository.GetSalesByManagerAndSalesIdAsync(managerUserId, salesUserId);

            if (salesUser == null)
                throw new UnauthorizedAccessException(
                    "You are not authorized to view this sales user"
                );

            return salesUser;
        }

    }
}
