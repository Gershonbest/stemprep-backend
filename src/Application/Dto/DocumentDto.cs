
namespace Application.Dto
{
    public class DocumentDto
    {
        public Guid DocumentId { get; set; }
        public string CloudinaryUrl { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public Guid UserId { get; set; }
    }
}
