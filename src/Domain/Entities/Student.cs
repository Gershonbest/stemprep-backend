using Domain.Common.Entities;

namespace Domain.Entities;
public class Student : BaseUser
{
    public string ParentEmail { get; set; }
    public string Username { get; set; } = string.Empty;
    public int StudentNumber { get; set; }
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    public Parent Parent { get; set; }
}