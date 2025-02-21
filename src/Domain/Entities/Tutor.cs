using Domain.Common.Entities;

namespace Domain.Entities
{
    public class Tutor : BaseUser
    {
        public ICollection<Document> Documents { get; set; }
    }
}
