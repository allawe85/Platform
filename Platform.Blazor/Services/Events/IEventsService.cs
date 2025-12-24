using Platform.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platform.Blazor.Services.Events
{
    public interface IEventsService
    {
        // Event Types
        Task<List<EventType>> GetEventTypesAsync();
        Task<EventType> GetEventTypeAsync(int id);
        Task<EventType> CreateEventTypeAsync(EventType eventType);
        Task UpdateEventTypeAsync(int id, EventType eventType);
        Task DeleteEventTypeAsync(int id);

        // Events
        Task<List<Event>> GetEventsAsync();
        Task<List<Event>> GetEventsByRangeAsync(DateTime start, DateTime end);
        Task<Event> GetEventAsync(int id);
        Task<Event> CreateEventAsync(Event eventItem);
        Task UpdateEventAsync(int id, Event eventItem);
        Task DeleteEventAsync(int id);
    }
}
