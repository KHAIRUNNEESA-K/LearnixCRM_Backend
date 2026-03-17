using LearnixCRM.Domain.Common;

namespace LearnixCRM.Domain.Entities
{
    public class Course : AuditableEntity
    {
        public int CourseId { get; private set; }

        public string Name { get; private set; } 
        public int CourseDuration { get; private set; }
        public decimal Fee { get; private set; }
        public bool IsActive { get; private set; }

        public ICollection<Lead> Leads { get; private set; } = new List<Lead>();

        private Course() { } // For EF


        public static Course Create(string name, decimal fee,int duration, int userId)
        {
            var course = new Course
            {
                Name = name,
                Fee = fee,
                CourseDuration=duration,
                IsActive = true
            };

            course.SetCreatedBy(userId);
            return course;
        }


        public void Update(string name, decimal fee,int duration, int userId)
        {
            Name = name;
            Fee = fee;
            CourseDuration=duration;
            SetUpdated(userId);
        }

        public void Delete(int userId)
        {
            SetDeleted(userId);
            IsActive = false;
        }

        public void Activate(int userId)
        {
            Restore();
            IsActive = true;
            SetUpdated(userId);
        }
    }
}
