using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;

namespace MtgPriceGuesser.Client.Pages
{
    public class GameModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GameModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public CardInfo CardA { get; set; } = new CardInfo();
        public CardInfo CardB { get; set; } = new CardInfo();

        public bool HasGuessed { get; set; } = false;
        public bool WasCorrect { get; set; } = false;

        public int Score { get; set; } = 0;
        public decimal Health { get; set; } = 40;
        public decimal NetWorth { get; set; } = 0;
        public bool IsGameOver { get; set; } = false;
        public bool ScoreSubmitted { get; set; } = false;

        public async Task OnGetAsync(int score = 0, decimal health = 40, decimal netWorth = 0)
        {
            Score = score;
            Health = health;
            NetWorth = netWorth;
            await LoadNewPair();
        }

        public async Task<IActionResult> OnPostGuessAsync(
            string chosenCard,
            string cardAName, string cardBName,
            decimal cardAPrice, decimal cardBPrice,
            string cardAImage, string cardBImage,
            string? cardABackImage, string? cardBBackImage,
            int currentScore, decimal currentHealth, decimal currentNetWorth)
        {
            CardA = new CardInfo { Name = cardAName, ImageUrl = cardAImage, BackImageUrl = cardABackImage, Price = cardAPrice };
            CardB = new CardInfo { Name = cardBName, ImageUrl = cardBImage, BackImageUrl = cardBBackImage, Price = cardBPrice };

            HasGuessed = true;

            bool aIsPricier = CardA.Price >= CardB.Price;
            WasCorrect = (chosenCard == "A" && aIsPricier) || (chosenCard == "B" && !aIsPricier);

            decimal pricierCardValue = aIsPricier ? CardA.Price : CardB.Price;

            Score = currentScore;
            Health = currentHealth;
            NetWorth = currentNetWorth;

            if (WasCorrect)
            {
                Score++;
                NetWorth += pricierCardValue;
            }
            else
            {
                Health -= pricierCardValue;
            }

            if (Health <= 0)
            {
                IsGameOver = true;
            }

            return Page();
        }

        private async Task LoadNewPair()
        {
            var client = _httpClientFactory.CreateClient("Api");

            var response = await client.GetAsync("api/cards/random-pair");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var cards = JsonSerializer.Deserialize<List<CardInfo>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (cards != null && cards.Count == 2)
            {
                CardA = cards[0];
                CardB = cards[1];
            }
        }

        public async Task<IActionResult> OnPostSubmitScoreAsync(string playerName, int finalScore, decimal finalNetWorth)
        {
            var client = _httpClientFactory.CreateClient("Api");

            var payload = new
            {
                playerName = playerName,
                score = finalScore,
                netWorth = finalNetWorth
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/highscores", content);
            response.EnsureSuccessStatusCode();

            Score = finalScore;
            NetWorth = finalNetWorth;
            IsGameOver = true;
            ScoreSubmitted = true;

            return Page();
        }
    }



    public class CardInfo
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string? BackImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}