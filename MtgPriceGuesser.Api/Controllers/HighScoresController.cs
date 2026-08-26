using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MtgPriceGuesser.Api.Data;
using MtgPriceGuesser.Api.Models;

namespace MtgPriceGuesser.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HighScoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HighScoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<HighScore>>> GetTopScores()
        {
            var topScores = await _context.HighScores
                .OrderByDescending(h => h.Score)
                .ThenByDescending(h => h.NetWorth)
                .Take(10)
                .ToListAsync();

            return Ok(topScores);
        }

        [HttpPost]
        public async Task<ActionResult<HighScore>> SubmitScore(HighScore newScore)
        {
            newScore.Id = 0; // ensure EF treats this as a new row, not an update
            newScore.DatePlayed = DateTime.UtcNow;

            _context.HighScores.Add(newScore);
            await _context.SaveChangesAsync();

            return Ok(newScore);
        }
    }
}
