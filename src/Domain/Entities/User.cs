using Domain.Common.Entities;

namespace Domain.Entities;
public class User : BaseUser
{
    public bool IsVerified { get; set; }
    
    public int? ProgrammeId { get; set; }

    public int StudentNumber { get; set; }
    public ICollection<Course>? EnrolledCourses { get; set; }

}