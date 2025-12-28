using Platform.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platform.Blazor.Services.Announcements
{
    public interface IAnnouncementsService
    {
        Task<List<Announcement>> GetAnnouncementsAsync();
        Task<Announcement> GetAnnouncementAsync(int id);
        Task<Announcement> CreateAnnouncementAsync(Announcement announcement);
        Task UpdateAnnouncementAsync(int id, Announcement announcement);
        Task DeleteAnnouncementAsync(int id);
    }
}
