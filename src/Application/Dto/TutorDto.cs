using AutoMapper;
using Domain.Entities;

namespace Application.Dto
{
    [AutoMap(typeof(Tutor))]
    public class TutorDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string AccountStatusDesc { get; set; }
        public string AvailabilityStatusDesc { get; set; }
        public ICollection<DocumentDto> Documents { get; set; }
    }
}
