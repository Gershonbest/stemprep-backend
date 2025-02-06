using Domain.Common.Entities;

namespace Domain.Entities;
public class Student : BaseUser
{

    public int? ProgrammeId { get; set; }

    public int StudentNumber { get; set; }
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    public ICollection<Course>? EnrolledCourses { get; set; }

}