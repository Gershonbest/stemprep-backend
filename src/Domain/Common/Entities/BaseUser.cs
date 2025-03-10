﻿using Domain.Enum;
namespace Domain.Common.Entities;
public abstract class BaseUser
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;

    public Status UserStatus { get; set; } = Status.Active;

    public string UserStatusDes { get; set; } = Status.Active.ToString();
    
    public UserType UserType { get; set; }

    public string UserTypeDesc { get; set; }
    public string Gender { get; set; }
    public string Email {  get; set; }
    public string Password { get; set; }
    public bool IsVerified { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
    
    public string? LastModifiedBy { get; set; } = string.Empty;
    
    public DateTime? LastModifiedDate { get; set; }
}
