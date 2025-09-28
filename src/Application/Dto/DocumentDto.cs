using AutoMapper;
using Domain.Entities;
using Domain.Enum;

namespace Application.Dto
{
    [AutoMap(typeof(Document))]
    public class DocumentDto
    {
        public Guid Guid { get; set; }
        public string CloudinaryUrl { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public Guid UserGuid { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentTypeDesc { get; set; }
    }
}
