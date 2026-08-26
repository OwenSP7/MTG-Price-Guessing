using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace MtgPriceGuesser.Client.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<HighScoreInfo> TopScores { get; set; } = new List<HighScoreInfo>();

        public async Task OnGetAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Api");
                var response = await client.GetAsync("api/highscores");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var scores = JsonSerializer.Deserialize<List<HighScoreInfo>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (scores != null)
                    {
                        TopScores = scores;
                    }
                }
            }
            catch (HttpRequestException)
            {
                // API might not be running — just show an empty list rather than crashing the page
            }
        }
    }

    public class HighScoreInfo
    {
        public string PlayerName { get; set; } = string.Empty;
        public int Score { get; set; }
        public decimal NetWorth { get; set; }
    }
}
