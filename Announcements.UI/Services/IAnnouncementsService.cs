using Announcements.UI.Models;
using Announcements.UI.Models.DTOs;

namespace Announcements.UI.Services
{
    public interface IAnnouncementsService
    {
        Task<Announcement> Read(string id);
        Task<List<Announcement>> ReadAll();
        Task<string> Create(AnnouncementDto item);
        Task Update(Announcement item);
        Task Delete(string id);
    }
}
