using AutoMapper;
using Domain.Entities;
using Domain.Enum;

namespace Application.Dto
{
    [AutoMap(typeof(Tutor))]
    public class TutorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public TutorAccountStatus AccountStatus { get; set; }
        public string AccountStatusDesc { get; set; }
        public AvailabilityStatus AvailabilityStatus { get; set; }
        public string AvailabilityStatusDesc { get; set; }
        public string ProfileUrl { get; set; }
    }
}
