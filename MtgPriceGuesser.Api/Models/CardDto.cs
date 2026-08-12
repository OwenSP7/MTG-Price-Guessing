namespace MtgPriceGuesser.Api.Models
{
    public class CardDto
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string? BackImageUrl { get; set; } // null for normal single-faced cards
        public decimal Price { get; set; }
    }
}
