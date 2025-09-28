
using Domain.Common.Entities;

namespace Domain.Entities
{
    public class Parent : BaseUser
    {
        public List<Student> Children { get; set; }
    }
}
