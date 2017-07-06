using CreditCards.Core.Interfaces;
using CreditCards.Core.Models;
using Moq;
using Xunit;

namespace CreditCards.Tests.Models
{
    public class CreditCardApplicationEvaluatorShould
    {
        private const int _expectedLowIncomeThreshhold = 20_000;
        private const int _expectedHighIncomeThreshhold = 100_000;
        private const string _validFrequentFlyerNumber = "012345-A";

        [Theory]
        [InlineData(_expectedHighIncomeThreshhold)]
        [InlineData(_expectedHighIncomeThreshhold + 1)]
        [InlineData(int.MaxValue)]
        public void AcceptAllHightIncomeApplications(int income)
        {
            // sut => System Under Test
            var sut = new CreditCardApplicationEvaluator(new FrequentFlyerNumberValidator());

            var application = new CreditCardApplication
            {
                FrequentFlyerNumber = _validFrequentFlyerNumber,
                GrossAnnualIncome = income
            };

            Assert.Equal(CreditCardApplicationDecision.AutoAccepted, sut.Evaluate(application));
        }
        
        [Theory]
        [InlineData(20)]
        [InlineData(19)]
        [InlineData(0)]
        [InlineData(int.MinValue)]
        public void ReferYoungApplicantsWhoAreNotHightIncome(int age)
        {
            // sut => System Under Test
            var sut = new CreditCardApplicationEvaluator(new FrequentFlyerNumberValidator());

            var application = new CreditCardApplication
            {
                Age = age,
                FrequentFlyerNumber = _validFrequentFlyerNumber,
                GrossAnnualIncome = _expectedHighIncomeThreshhold - 1
            };

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, sut.Evaluate(application));
        }

        [Theory]
        [InlineData(_expectedLowIncomeThreshhold)]
        [InlineData(_expectedLowIncomeThreshhold + 1)]
        [InlineData(_expectedHighIncomeThreshhold - 1)]
        public void ReferNonYoungApplicantsWhoAreMiddleIncome(int income)
        {
            // sut => System Under Test
            var sut = new CreditCardApplicationEvaluator(new FrequentFlyerNumberValidator());

            var application = new CreditCardApplication
            {
                Age = 21,
                FrequentFlyerNumber = _validFrequentFlyerNumber,
                GrossAnnualIncome = income
            };

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, sut.Evaluate(application));
        }

        [Theory]
        [InlineData(_expectedLowIncomeThreshhold - 1)]
        [InlineData(0)]
        [InlineData(int.MinValue)]
        public void DeclineAllApplicantsWhoAreLowIncome(int income)
        {
            // sut => System Under Test
            var sut = new CreditCardApplicationEvaluator(new FrequentFlyerNumberValidator());

            var application = new CreditCardApplication
            {
                Age = 21,
                FrequentFlyerNumber = _validFrequentFlyerNumber,
                GrossAnnualIncome = income
            };

            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, sut.Evaluate(application));
        }

        [Fact]
        public void ReferInvalidFrequentFlyerNumbers_RealValidator()
        {
            // sut => System Under Test
            var sut = new CreditCardApplicationEvaluator(new FrequentFlyerNumberValidator());

            var application = new CreditCardApplication
            {
                FrequentFlyerNumber = "0dm389dn29"
            };

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, sut.Evaluate(application));
        }

        [Fact]
        public void ReferInvalidFrequentFlyerNumbers_MockValidator()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);

            // sut => System Under Test
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication();

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, sut.Evaluate(application));
        }
    }
}
