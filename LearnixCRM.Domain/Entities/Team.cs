using LearnixCRM.Domain.Common;

namespace LearnixCRM.Domain.Entities
{
    public class Team : AuditableEntity
    {
        public int TeamId { get; private set; }

        public string TeamName { get; private set; }

        public int ManagerUserId { get; private set; }

        public User ManagerUser { get; private set; }

        public bool IsActive { get; private set; }

        public ICollection<AssignUsers> Members { get; private set; } = new List<AssignUsers>();

        protected Team() { }

        public static Team Create(string teamName, int managerUserId, int createdBy)
        {
            var team = new Team
            {
                TeamName = teamName,
                ManagerUserId = managerUserId,
                IsActive = true
            };

            team.SetCreatedBy(createdBy);

            return team;
        }

        public void ChangeName(string name, int updatedBy)
        {
            TeamName = name;
            SetUpdated(updatedBy);
        }

        public void ChangeManager(int managerUserId, int updatedBy)
        {
            ManagerUserId = managerUserId;
            SetUpdated(updatedBy);
        }

        public void Deactivate(int deletedBy)
        {
            IsActive = false;
            SetDeleted(deletedBy);
        }
    }
}