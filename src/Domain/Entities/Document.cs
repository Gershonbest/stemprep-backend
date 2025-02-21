using Domain.Common.Entities;
using Domain.Enum;

namespace Domain.Entities
{
    public class Document : BaseEntity
    {
        public string CloudinaryUrl { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; } 
        public Guid UserGuId { get; set; }
        public UserType UserType { get; set; }
        public string UserTypeDesc { get; set; }
    }
}
