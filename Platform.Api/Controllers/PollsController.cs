using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.Data;
using Platform.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        private readonly PlatformDbContext _context;

        public PollsController(PlatformDbContext context)
        {
            _context = context;
        }

        // GET: api/Polls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Poll>>> GetPolls()
        {
            return await _context.GetAllPollsAsync();
        }

        // GET: api/Polls/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Poll>>> GetActivePolls()
        {
            // We can add a method in DbContext or filter here. 
            // Better to use DbContext logic if available or just filter GetAllPollsAsync if performance allows, 
            // but I'll add a filter here for simplicity as I didn't verify GetActivePollsAsync in DbContext explicitly in the final replace (I might have missed it or it was in the first failed attempt).
            // Actually, I didn't add GetActivePollsAsync in the final successful DbContext update. I added GetAllPollsAsync.
            // So I will filter here or add a new method. Filtering here is fine for "Simple" polls.
            var polls = await _context.GetAllPollsAsync(); // This includes Options
            var activePolls = polls.Where(p => p.IsActive).ToList();
            return activePolls;
        }

        // GET: api/Polls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Poll>> GetPoll(int id)
        {
            var poll = await _context.GetPollByIdAsync(id);

            if (poll == null)
            {
                return NotFound();
            }

            return poll;
        }

        // POST: api/Polls
        [HttpPost]
        public async Task<ActionResult<Poll>> PostPoll(Poll poll)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(poll.Question) || poll.Options == null || poll.Options.Count < 2)
            {
                return BadRequest("Poll must have a question and at least 2 options.");
            }

            var createdPoll = await _context.AddPollAsync(poll);
            return CreatedAtAction("GetPoll", new { id = createdPoll.Id }, createdPoll);
        }

        // PUT: api/Polls/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoll(int id, Poll poll)
        {
            if (id != poll.Id)
            {
                return BadRequest();
            }

            var updatedPoll = await _context.UpdatePollAsync(poll);

            if (updatedPoll == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Polls/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoll(int id)
        {
            var result = await _context.DeletePollAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Polls/vote
        [HttpPost("vote")]
        public async Task<ActionResult<PollVote>> Vote(PollVote vote)
        {
            // Verify Poll exists and is active
            var poll = await _context.GetPollByIdAsync(vote.PollId);
            if (poll == null || !poll.IsActive)
            {
                return BadRequest("Poll not found or not active.");
            }

            // Verify Employee hasn't voted yet
            var hasVoted = await _context.HasEmployeeVotedAsync(vote.PollId, vote.EmployeeId);
            if (hasVoted)
            {
                return BadRequest("Employee has already voted on this poll.");
            }

            try 
            {
                // Ensure correct date
                vote.VotedAt = DateTime.Now;
                var createdVote = await _context.AddPollVoteAsync(vote);
                return Ok(createdVote); // Or CreatedAtAction if we had a GetVote
            }
            catch (Exception ex)
            {
                return BadRequest($"Error recording vote: {ex.Message}");
            }
        }

        // GET: api/Polls/5/results
        [HttpGet("{id}/results")]
        public async Task<ActionResult<Dictionary<int, int>>> GetPollResults(int id)
        {
            var results = await _context.GetPollResultsAsync(id);
            return results;
        }
    }
}
