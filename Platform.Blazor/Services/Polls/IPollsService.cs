using Platform.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platform.Blazor.Services.Polls
{
    public interface IPollsService
    {
        Task<List<Poll>> GetAllPollsAsync();
        Task<List<Poll>> GetActivePollsAsync();
        Task<Poll> GetPollByIdAsync(int id);
        Task<Poll> AddPollAsync(Poll poll);
        Task UpdatePollAsync(int id, Poll poll);
        Task DeletePollAsync(int id);
        Task<PollVote> VoteAsync(PollVote vote);
        Task<Dictionary<int, int>> GetPollResultsAsync(int id);
    }
}
