using Domain.Common.Entities;

namespace Domain.Entities
{
    public class User : BaseUser
    {
        public bool IsVerified { get; set; }
    }
}
