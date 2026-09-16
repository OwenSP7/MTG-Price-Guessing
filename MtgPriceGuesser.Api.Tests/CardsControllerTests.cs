using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MtgPriceGuesser.Api.Controllers;
using Xunit;

namespace MtgPriceGuesser.Api.Tests
{
    public class CardsControllerTests
    {
        [Fact]
        public void HasValidPrice_ReturnsTrue_ForValidPositivePrice()
        {
            bool result = CardsController.HasValidPrice("5.99");
            Assert.True(result);
        }

        [Fact]
        public void HasValidPrice_ReturnsFalse_ForZeroPrice()
        {
            bool result = CardsController.HasValidPrice("0");
            Assert.False(result);
        }

        [Fact]
        public void HasValidPrice_ReturnsFalse_ForNullPrice()
        {
            bool result = CardsController.HasValidPrice(null);
            Assert.False(result);
        }

        [Fact]
        public void HasValidPrice_ReturnsFalse_ForNonNumericString()
        {
            bool result = CardsController.HasValidPrice("not a number");
            Assert.False(result);
        }
    }
}
