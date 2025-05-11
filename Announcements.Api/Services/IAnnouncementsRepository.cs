using Announcements.Api.Models;

namespace Announcements.Api.Services
{
    public interface IAnnouncementsRepository
    {
        Task<Announcement> Read(string id);
        Task<List<Announcement>> ReadAll();
        Task Create(Announcement item);
        Task Update(Announcement item);
        Task Delete(string id);
    }
}