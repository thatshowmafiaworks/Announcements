using System.ComponentModel.DataAnnotations;

namespace Announcements.UI.Models.DTOs
{
    public class AnnouncementDto
    {
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string SubCategory { get; set; }
    }
}
