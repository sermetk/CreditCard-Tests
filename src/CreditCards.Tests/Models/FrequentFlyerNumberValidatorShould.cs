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
        
        [Theory]
        [InlineData("012345-1")]
        [InlineData("012345-B")]
        [InlineData("012345-P")]
        [InlineData("012345-R")]
        [InlineData("012345-X")]
        [InlineData("012345-Z")]
        [InlineData("012345- ")]
        [InlineData("012345-a")]
        [InlineData("012345-q")]
        [InlineData("012345-y")]
        public void RejectInvalidScheme(string number)
        {
            // sut => System Under Test
            var sut = new FrequentFlyerNumberValidator();

            Assert.False(sut.IsValid(number));
        }

        [Theory]
        [InlineData("0012345-A")]
        [InlineData("X12345-A")]
        [InlineData("01234X-A")]
        public void RejectInvalidNumber(string number)
        {
            // sut => System Under Test
            var sut = new FrequentFlyerNumberValidator();

            Assert.False(sut.IsValid(number));
        }

        [Theory]
        [InlineData("      -A")]
        [InlineData("  1   -A")]
        [InlineData("1     -A")]
        [InlineData("     1-A")]
        public void RejectEmptyMemberNumberDigit(string number)
        {
            // sut => System Under Test
            var sut = new FrequentFlyerNumberValidator();

            Assert.False(sut.IsValid(number));
        }

        [Theory]
        [InlineData("        ")]
        [InlineData("")]
        public void RejectEmptyFrequentMemberNumber(string number)
        {
            // sut => System Under Test
            var sut = new FrequentFlyerNumberValidator();

            Assert.False(sut.IsValid(number));
        }

        [Fact]
        public void ThrowExceptionWhenNullFrequentFlyerNumber()
        {
            // sut => System Under Test
            var sut = new FrequentFlyerNumberValidator();

            Assert.Throws<ArgumentNullException>(() => sut.IsValid(null));
        }
    }
}
