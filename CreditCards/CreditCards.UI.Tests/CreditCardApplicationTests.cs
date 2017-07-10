using CreditCards.UI.Tests.PageObjectModels;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Xunit;

namespace CreditCards.UI.Tests
{
    public class CreditCardApplicationTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private ApplicationPage _applicationPage;

        public CreditCardApplicationTests()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("http://localhost:64559/");
            _applicationPage = new ApplicationPage(_driver);
            _applicationPage.NavigateTo();
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [Fact]
        public void ShouldLoadApplicationPage_SmokeTest()
        {
            Assert.Equal("Credit Card Application - CreditCards", _applicationPage.Driver.Title);
        }

        [Fact]
        public void ShouldValidateApplicationDetails()
        {
            _applicationPage.EnterName("", "Smith");
            _applicationPage.EnterFrequentFlyerNumber("012345-A");
            _applicationPage.EnterAge("20");
            _applicationPage.EnterGrossAnnualIncome("100000");
            _applicationPage.SubmitApplication();

            Assert.Equal("Credit Card Application - CreditCards", _applicationPage.Driver.Title);
            Assert.Equal("Please provide a first name", _applicationPage.FirstErrorMessage);
        }

        [Fact]
        public void ShouldDeclineLowIncomes()
        {
            _applicationPage.EnterName("Sarah", "Smith");
            _applicationPage.EnterFrequentFlyerNumber("012345-A");
            _applicationPage.EnterAge("35");
            _applicationPage.EnterGrossAnnualIncome("10000");
            var applicationCompletePage = _applicationPage.SubmitApplication();

            Assert.Equal("Application Complete - CreditCards", applicationCompletePage.Driver.Title);
            var applicationDecision = _driver.FindElement(By.Id("decision"));
            Assert.Equal("AutoDeclined", applicationCompletePage.ApplicationDecision);
        }
    }
}
