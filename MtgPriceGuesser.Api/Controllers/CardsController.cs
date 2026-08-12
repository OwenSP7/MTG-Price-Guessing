using Microsoft.AspNetCore.Mvc;
using MtgPriceGuesser.Api.Models;
using System.Text.Json;

namespace MtgPriceGuesser.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CardsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("random-pair")]
        public async Task<ActionResult<List<CardDto>>> GetRandomPair()
        {
            var cardATask = GetRandomCardWithPrice();
            var cardBTask = GetRandomCardWithPrice();

            await Task.WhenAll(cardATask, cardBTask);

            return Ok(new List<CardDto> { cardATask.Result, cardBTask.Result });
        }

        private async Task<CardDto> GetRandomCardWithPrice()
        {
            var client = _httpClientFactory.CreateClient("Scryfall");

            int maxAttempts = 10;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                if (attempt > 0)
                {
                    await Task.Delay(30);
                }

                HttpResponseMessage response;
                try
                {
                    response = await client.GetAsync("cards/random?q=usd%3E0");
                }
                catch (TaskCanceledException)
                {
                    continue; // this specific request timed out (per Step 1's 5s limit) — just try again
                }

                if (!response.IsSuccessStatusCode)
                {
                    continue;
                }

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var priceElement = root.GetProperty("prices").GetProperty("usd");

                if (priceElement.ValueKind != JsonValueKind.String)
                {
                    continue;
                }

                string? imageUrl = null;
                string? backImageUrl = null;

                if (root.TryGetProperty("image_uris", out var imageUris))
                {
                    imageUrl = imageUris.GetProperty("normal").GetString();
                }
                else if (root.TryGetProperty("card_faces", out var cardFaces) &&
                          cardFaces.GetArrayLength() >= 2 &&
                          cardFaces[0].TryGetProperty("image_uris", out var frontFaceImages) &&
                          cardFaces[1].TryGetProperty("image_uris", out var backFaceImages))
                {
                    imageUrl = frontFaceImages.GetProperty("normal").GetString();
                    backImageUrl = backFaceImages.GetProperty("normal").GetString();
                }

                if (string.IsNullOrEmpty(imageUrl))
                {
                    continue;
                }

                return new CardDto
                {
                    Name = root.GetProperty("name").GetString() ?? "Unknown",
                    ImageUrl = imageUrl,
                    BackImageUrl = backImageUrl,
                    Price = decimal.Parse(priceElement.GetString()!)
                };
            }

            throw new Exception("Could not find a valid priced card after multiple attempts.");
        }
    }
}


