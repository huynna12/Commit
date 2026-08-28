using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Commit.Api.Data;
using System.Security.Claims;
using Commit.Api.Models.DTOs;
using Commit.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Commit.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChallengesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChallengesController(AppDbContext context)
        {
            _context = context;
        }
        private static ChallengeDto ChallengeToDto(Challenge challenge) => new ChallengeDto
        {
            Id = challenge.Id,
            Title = challenge.Title,
            Description = challenge.Description,
            StartDate = challenge.StartDate,
            DurationInDays = challenge.DurationInDays,
            ScheduleDays = challenge.ScheduleDays,
            JoinPolicy = challenge.JoinPolicy,
            MaxParticipants = challenge.MaxParticipants,
            OwnerId = challenge.OwnerId
        };

        [HttpPost]
        public async Task<ActionResult<ChallengeDto>> CreateChallenge(CreateChallengeDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var challenge = new Challenge
            {
                Title = dto.Title,
                Description = dto.Description,
                StartDate = dto.StartDate,
                DurationInDays = dto.DurationInDays,
                MaxParticipants = dto.MaxParticipants,
                JoinPolicy = dto.JoinPolicy,
                ScheduleDays = dto.ScheduleDays,
                OwnerId = userId
            };

            _context.Challenges.Add(challenge);
            await _context.SaveChangesAsync();

            var participants = new ChallengeParticipant
            {
                ChallengeId = challenge.Id,
                AppUserId = userId,
            };

            _context.ChallengeParticipants.Add(participants);
            await _context.SaveChangesAsync();

            var result = ChallengeToDto(challenge);

            return CreatedAtAction(nameof(GetChallenge), new { id = challenge.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChallengeDto>> GetChallenge(int id)
        {
            var challenge = await _context.Challenges.FindAsync(id);
            
            if (challenge == null)
            {
                return NotFound();
            }
            return ChallengeToDto(challenge);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChallenge(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var challenge = await _context.Challenges.FindAsync(id);

            if (challenge == null) return NotFound();

            if (challenge.OwnerId != userId) return Forbid();
            _context.Challenges.Remove(challenge);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<List<ChallengeDto>>> GetUserChallenges()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var challenges = await _context.ChallengeParticipants
                                .Where(cp => cp.AppUserId == userId)
                                .Select(cp => cp.Challenge)
                                .ToListAsync();
            return challenges.Select(ChallengeToDto).ToList();
        }

        [HttpPost("{id}/join")]
        public async Task<IActionResult> JoinChallenge(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null) return NotFound();  

            var existingParticipant = await _context.ChallengeParticipants
                .FirstOrDefaultAsync(cp => cp.ChallengeId == id && cp.AppUserId == userId);
            if (existingParticipant != null) return BadRequest("Already joined this challenge.");

            var count = await _context.ChallengeParticipants.CountAsync(cp => cp.ChallengeId == id);
            if (challenge.MaxParticipants != null && count >= challenge.MaxParticipants) return BadRequest("Challenge is full.");

            switch (challenge.JoinPolicy) 
            {
                case JoinPolicy.Open:
                    var participant = new ChallengeParticipant
                    {
                        ChallengeId = id,
                        AppUserId = userId
                    };
                    _context.ChallengeParticipants.Add(participant);
                    await _context.SaveChangesAsync();
                    return Ok("Joined challenge successfully.");    
                case JoinPolicy.InviteOnly:
                    return BadRequest("This challenge is invite-only.");
                case JoinPolicy.RequiresApproval:
                    var hasPending = await _context.JoinRequests
                        .AnyAsync(jr => jr.AppUserId == userId
                                     && jr.ChallengeId == id
                                     && jr.RequestStatus == Status.Pending);
                    if (hasPending) return BadRequest("You have already requested to join this challenge."); 
                    
                    var joinRequest = new JoinRequest
                    {
                        ChallengeId = id,
                        AppUserId = userId,
                        RequestStatus = Status.Pending
                    };
                    _context.JoinRequests.Add(joinRequest);
                    await _context.SaveChangesAsync();
                    return Ok("Join request submitted successfully.");

                default: 
                    return BadRequest("Invalid join policy.");
            }
        }
    }
}
