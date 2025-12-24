using Platform.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Platform.Blazor.Services.Events
{
    public class EventsService : IEventsService
    {
        private readonly HttpClient _http;

        public EventsService(HttpClient http)
        {
            _http = http;
        }

        // --- Event Types ---

        public async Task<List<EventType>> GetEventTypesAsync()
        {
            return await _http.GetFromJsonAsync<List<EventType>>("api/EventTypes");
        }

        public async Task<EventType> GetEventTypeAsync(int id)
        {
            return await _http.GetFromJsonAsync<EventType>($"api/EventTypes/{id}");
        }

        public async Task<EventType> CreateEventTypeAsync(EventType eventType)
        {
            var response = await _http.PostAsJsonAsync("api/EventTypes", eventType);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<EventType>();
        }

        public async Task UpdateEventTypeAsync(int id, EventType eventType)
        {
            var response = await _http.PutAsJsonAsync($"api/EventTypes/{id}", eventType);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteEventTypeAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/EventTypes/{id}");
            response.EnsureSuccessStatusCode();
        }

        // --- Events ---

        public async Task<List<Event>> GetEventsAsync()
        {
            return await _http.GetFromJsonAsync<List<Event>>("api/Events");
        }

        public async Task<List<Event>> GetEventsByRangeAsync(DateTime start, DateTime end)
        {
            // Format dates as ISO 8601 strings
            string startStr = start.ToString("yyyy-MM-ddTHH:mm:ss");
            string endStr = end.ToString("yyyy-MM-ddTHH:mm:ss");
            return await _http.GetFromJsonAsync<List<Event>>($"api/Events/range?start={startStr}&end={endStr}");
        }

        public async Task<Event> GetEventAsync(int id)
        {
            return await _http.GetFromJsonAsync<Event>($"api/Events/{id}");
        }

        public async Task<Event> CreateEventAsync(Event eventItem)
        {
            var response = await _http.PostAsJsonAsync("api/Events", eventItem);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Event>();
        }

        public async Task UpdateEventAsync(int id, Event eventItem)
        {
            var response = await _http.PutAsJsonAsync($"api/Events/{id}", eventItem);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteEventAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/Events/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
