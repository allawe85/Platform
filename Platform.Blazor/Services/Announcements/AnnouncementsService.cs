using Platform.Data.DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Platform.Blazor.Services.Announcements
{
    public class AnnouncementsService : IAnnouncementsService
    {
        private readonly HttpClient _http;

        public AnnouncementsService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Announcement>> GetAnnouncementsAsync()
        {
            return await _http.GetFromJsonAsync<List<Announcement>>("api/Announcements");
        }

        public async Task<Announcement> GetAnnouncementAsync(int id)
        {
            return await _http.GetFromJsonAsync<Announcement>($"api/Announcements/{id}");
        }

        public async Task<Announcement> CreateAnnouncementAsync(Announcement announcement)
        {
            var response = await _http.PostAsJsonAsync("api/Announcements", announcement);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Announcement>();
        }

        public async Task UpdateAnnouncementAsync(int id, Announcement announcement)
        {
            var response = await _http.PutAsJsonAsync($"api/Announcements/{id}", announcement);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAnnouncementAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/Announcements/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
