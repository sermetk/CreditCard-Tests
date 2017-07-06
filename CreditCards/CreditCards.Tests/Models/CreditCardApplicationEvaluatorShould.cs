using CreditCards.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CreditCards.Tests.Models
{
    public class CreditCardApplicationEvaluatorShould
    {
        private const int _expectedLowIncomeThreshhold = 20_000;
        private const int _expectedHighIncomeThreshhold = 100_000;

        [Theory]
        [InlineData(_expectedHighIncomeThreshhold)]
        [InlineData(_expectedHighIncomeThreshhold + 1)]
        [InlineData(int.MaxValue)]
        public void AcceptAllHightIncomeApplications(int income)
        {
            // sut => System Under Test
            var sut = new CreditCardApplicationEvaluator();

            var application = new CreditCardApplication
            {
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
            var sut = new CreditCardApplicationEvaluator();

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = _expectedHighIncomeThreshhold - 1,
                Age = age
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
            var sut = new CreditCardApplicationEvaluator();

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = income,
                Age = 21
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
            var sut = new CreditCardApplicationEvaluator();

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = income,
                Age = 21
            };

            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, sut.Evaluate(application));
        }
    }
}
