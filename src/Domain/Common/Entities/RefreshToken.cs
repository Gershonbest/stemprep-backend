namespace Domain.Common.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        // Foreign key relationship
        public int? TutorId { get; set; }
        public int? ParentId { get; set; }
        public int? AdminId { get; set; }
        public int? StudentId { get; set; }
    }
}
