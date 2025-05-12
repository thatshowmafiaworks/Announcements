namespace Announcements.UI.Models
{
    public class Announcement
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
    }
}
