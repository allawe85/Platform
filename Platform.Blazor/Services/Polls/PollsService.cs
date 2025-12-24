using Platform.Data.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Platform.Blazor.Services.Polls
{
    public class PollsService : IPollsService
    {
        private readonly HttpClient _httpClient;

        public PollsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Poll>> GetAllPollsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Poll>>("api/Polls");
        }

        public async Task<List<Poll>> GetActivePollsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Poll>>("api/Polls/active");
        }

        public async Task<Poll> GetPollByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Poll>($"api/Polls/{id}");
        }

        public async Task<Poll> AddPollAsync(Poll poll)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Polls", poll);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Poll>();
        }

        public async Task UpdatePollAsync(int id, Poll poll)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Polls/{id}", poll);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeletePollAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Polls/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<PollVote> VoteAsync(PollVote vote)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Polls/vote", vote);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PollVote>();
        }

        public async Task<Dictionary<int, int>> GetPollResultsAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Dictionary<int, int>>($"api/Polls/{id}/results");
        }
    }
}
