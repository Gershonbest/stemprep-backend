using Domain.Enum;
using Microsoft.AspNetCore.Identity;
namespace Domain.Common.Entities;
public abstract class BaseUser : IdentityUser
{
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;

    public Status UserStatus { get; set; } = Status.Active;

    public string UserStatusDes { get; set; } = Status.Active.ToString();
    
    public UserType UserType { get; set; }

    public string UserTypeDesc { get; set; }
    
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
    
    public string? LastModifiedBy { get; set; } = string.Empty;
    
    public DateTime? LastModifiedDate { get; set; }
}
