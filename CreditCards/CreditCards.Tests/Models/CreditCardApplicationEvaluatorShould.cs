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

        private readonly Mock<IFrequentFlyerNumberValidator> _mockValidator;
        // sut => System Under Test
        private readonly CreditCardApplicationEvaluator _sut;

        public CreditCardApplicationEvaluatorShould()
        {
            _mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            _mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            _sut = new CreditCardApplicationEvaluator(_mockValidator.Object);
        }

        [Theory]
        [InlineData(_expectedHighIncomeThreshhold)]
        [InlineData(_expectedHighIncomeThreshhold + 1)]
        [InlineData(int.MaxValue)]
        public void AcceptAllHightIncomeApplications(int income)
        {
            var application = new CreditCardApplication
            {
                GrossAnnualIncome = income
            };

            Assert.Equal(CreditCardApplicationDecision.AutoAccepted, _sut.Evaluate(application));
        }
        
        [Theory]
        [InlineData(20)]
        [InlineData(19)]
        [InlineData(0)]
        [InlineData(int.MinValue)]
        public void ReferYoungApplicantsWhoAreNotHightIncome(int age)
        {
            var application = new CreditCardApplication
            {
                Age = age,
                GrossAnnualIncome = _expectedHighIncomeThreshhold - 1
            };

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, _sut.Evaluate(application));
        }

        [Theory]
        [InlineData(_expectedLowIncomeThreshhold)]
        [InlineData(_expectedLowIncomeThreshhold + 1)]
        [InlineData(_expectedHighIncomeThreshhold - 1)]
        public void ReferNonYoungApplicantsWhoAreMiddleIncome(int income)
        {
            var application = new CreditCardApplication
            {
                Age = 21,
                GrossAnnualIncome = income
            };

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, _sut.Evaluate(application));
        }

        [Theory]
        [InlineData(_expectedLowIncomeThreshhold - 1)]
        [InlineData(0)]
        [InlineData(int.MinValue)]
        public void DeclineAllApplicantsWhoAreLowIncome(int income)
        {
            var application = new CreditCardApplication
            {
                Age = 21,
                GrossAnnualIncome = income
            };

            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, _sut.Evaluate(application));
        }

        [Fact]
        public void ReferInvalidFrequentFlyerNumbers()
        {
            _mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);

            var application = new CreditCardApplication();

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, _sut.Evaluate(application));
        }
    }
}
