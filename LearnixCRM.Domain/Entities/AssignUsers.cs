using LearnixCRM.Domain.Common;
using LearnixCRM.Domain.Entities;

public class AssignUsers : AuditableEntity
{
    public int AssignId { get; private set; }

    public int TeamId { get; private set; }
    public Team Team { get; private set; }

    public int SalesUserId { get; private set; }
    public User SalesUser { get; private set; }

    public bool IsActive { get; private set; }

    protected AssignUsers() { }

    public static AssignUsers Create(
        int teamId,
        int salesUserId,
        int createdBy)
    {
        var assignment = new AssignUsers
        {
            TeamId = teamId,
            SalesUserId = salesUserId,
            IsActive = true
        };

        assignment.SetCreatedBy(createdBy);

        return assignment;
    }

    public void Deactivate(int deletedBy)
    {
        IsActive = false;
        SetDeleted(deletedBy);
    }

    public void Delete(int deletedBy)
    {
        if (IsDeleted)
            return;

        IsActive = false;
        SetDeleted(deletedBy);
    }
}