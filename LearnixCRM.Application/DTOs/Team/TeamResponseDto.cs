namespace LearnixCRM.Application.DTOs.Team
{
    public class TeamResponseDto
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public int ManagerUserId { get; set; }

        public string ManagerName { get; set; }
    }
}