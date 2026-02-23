using LearnixCRM.Domain.Common;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;

public class Student : AuditableEntity
{
    public int StudentId { get; private set; }

    public int LeadId { get; private set; }

    public string FullName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string? Phone { get; private set; }

    public CourseType Course { get; private set; }

    public DateTime JoinedDate { get; private set; }

    public Lead Lead { get; private set; } = null!;

    private Student() { }

    public Student(Lead lead, DateTime joinedDate, int createdBy)
    {
        LeadId = lead.LeadId;
        FullName = lead.FullName;
        Email = lead.Email;
        Phone = lead.Phone;
        Course = lead.CourseInterested;
        JoinedDate = joinedDate;

        SetCreatedBy(createdBy);
    }
}