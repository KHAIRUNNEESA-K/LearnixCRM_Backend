namespace LearnixCRM.Application.DTOs.Team
{
    public class UpdateTeamRequestDto
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public int ManagerUserId { get; set; }
    }
}