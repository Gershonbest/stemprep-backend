using Domain.Common.Entities;
using Domain.Enum;

namespace Domain.Entities
{
    public class Tutor : BaseUser
    {
        public ICollection<Document> Documents { get; set; }
        public TutorAccountStatus AccountStatus { get; set; }
        public string AccountStatusDesc { get; set; }
        public AvailabilityStatus AvailabilityStatus { get; set; }
        public string AvailabilityStatusDesc { get; set; }
    }
}
