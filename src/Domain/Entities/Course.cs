using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using Domain.Common.Entities;
using Domain.Enum;


namespace Domain.Entities
{
    public class Course : BaseEntity
    {
        public string? Title { get; set; }

        public string? CourseImageUrl { get; set; }

        public required string Description { get; set; }

        public required string Objectives { get; set; }

        public required string InstructorName { get; set; }

        [ForeignKey(nameof(InstructorId))]
        public string? InstructorId { get; set; }

        public ModuleStatus ModuleStatus { get; set; } = ModuleStatus.Pending;

        public string ModuleStatusDes { get; set; } = ModuleStatus.Pending.ToString();

    }
}