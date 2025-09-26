using AutoMapper;
using Domain.Common.Entities;
using Domain.Enum;

namespace Application.Dto
{
    [AutoMap(typeof(BaseUser))]
    public class UserDto
    {
        public Guid Guid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Status UserStatus { get; set; }

        public string UserStatusDes { get; set; }

        public UserType UserType { get; set; }

        public string UserTypeDesc { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Province { get; set; }
    }
}
