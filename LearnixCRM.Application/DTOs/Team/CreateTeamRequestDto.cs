namespace LearnixCRM.Application.DTOs.Team
{
    public class CreateTeamRequestDto
    {
        public string TeamName { get; set; }

        public int ManagerUserId { get; set; }
    }
}