using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MtgPriceGuesser.Client.Pages;
using Xunit;

namespace MtgPriceGuesser.Client.Tests
{
    public class GameModelTests
    {
        [Fact]
        public void CheckGuessCorrect_ReturnsTrue_WhenCardAIsPricierAndChosen()
        {
            bool result = GameModel.CheckGuessCorrect("A", 10.00m, 5.00m);
            Assert.True(result);
        }

        [Fact]
        public void CheckGuessCorrect_ReturnsFalse_WhenCardAIsPricierButBChosen()
        {
            bool result = GameModel.CheckGuessCorrect("B", 10.00m, 5.00m);
            Assert.False(result);
        }

        [Fact]
        public void CheckGuessCorrect_ReturnsTrue_WhenCardBIsPricierAndChosen()
        {
            bool result = GameModel.CheckGuessCorrect("B", 5.00m, 10.00m);
            Assert.True(result);
        }

        [Fact]
        public void CheckGuessCorrect_ReturnsTrue_WhenPricesAreEqual_AIsTreatedAsPricier()
        {
            // Matches the >= tie-breaking rule used in GameModel
            bool result = GameModel.CheckGuessCorrect("A", 10.00m, 10.00m);
            Assert.True(result);
        }
    }
}
