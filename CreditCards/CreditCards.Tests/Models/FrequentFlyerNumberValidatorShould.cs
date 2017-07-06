using CreditCards.Core.Models;
using System;
using Xunit;

namespace CreditCards.Tests.Models
{
    public class FrequentFlyerNumberValidatorShould
    {
        [Theory]
        [InlineData("012345-A")]
        [InlineData("012345-Q")]
        [InlineData("012345-Y")]
        public void AcceptValidSchemes(string number)
        {
            // sut => System Under Test
            var sut = new FrequentFlyerNumberValidator();

            Assert.True(sut.IsValid(number));
        }
    }
}
