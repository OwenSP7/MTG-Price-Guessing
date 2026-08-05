using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MtgPriceGuesser.Client.Pages
{
    public class GameModel : PageModel
    {
        public CardInfo CardA { get; set; } = new CardInfo
        {
            Name = "Vincent Valentine",
            ImageUrl = "https://cards.scryfall.io/display/front/1/5/15ea1113-7360-462d-91b8-22d5110cbf5a.webp?1783906452"
        };

        public CardInfo CardB { get; set; } = new CardInfo
        {
            Name = "NOT A WOLF",
            ImageUrl = "https://cards.scryfall.io/display/front/5/4/54c92a0d-ab6e-45b2-b7a8-e13a5dd4e74e.webp?1783911045"
        };

        public void OnGet()
        {
        }
    }

    public class CardInfo
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
