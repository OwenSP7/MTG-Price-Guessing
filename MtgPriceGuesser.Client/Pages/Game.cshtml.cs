using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

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

        public async Task OnGetAsync()
        {
            await LoadNewPair();
        }

        public async Task<IActionResult> OnPostGuessAsync(
            string chosenCard,
            string cardAName, string cardBName,
            decimal cardAPrice, decimal cardBPrice,
            string cardAImage, string cardBImage,
            string? cardABackImage, string? cardBBackImage)
        {
            CardA = new CardInfo { Name = cardAName, ImageUrl = cardAImage, BackImageUrl = cardABackImage, Price = cardAPrice };
            CardB = new CardInfo { Name = cardBName, ImageUrl = cardBImage, BackImageUrl = cardBBackImage, Price = cardBPrice };

            HasGuessed = true;

            bool aIsPricier = CardA.Price >= CardB.Price;
            WasCorrect = (chosenCard == "A" && aIsPricier) || (chosenCard == "B" && !aIsPricier);

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
    }

    public class CardInfo
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string? BackImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}