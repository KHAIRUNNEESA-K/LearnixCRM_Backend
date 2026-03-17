using LearnixCRM.Application.DTOs.User;

namespace LearnixCRM.Application.DTOs.Team
{
    public class TeamWithMembersResponseDto
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public string ManagerName { get; set; }

        public List<SalesUserDto> SalesMembers { get; set; }
    }
}