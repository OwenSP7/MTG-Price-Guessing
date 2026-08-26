using System.ComponentModel.DataAnnotations;

namespace MtgPriceGuesser.Api.Models
{
    public class HighScore
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string PlayerName { get; set; } = string.Empty;
        public int Score { get; set; }
        public decimal NetWorth { get; set; }
        public DateTime DatePlayed { get; set; } = DateTime.UtcNow;
    }
}
