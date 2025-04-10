using AutoMapper;
using Domain.Entities;

namespace Application.Dto
{
    [AutoMap(typeof(Document))]
    public class DocumentDto
    {
        public Guid Guid { get; set; }
        public string CloudinaryUrl { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public Guid UserId { get; set; }
    }
}
