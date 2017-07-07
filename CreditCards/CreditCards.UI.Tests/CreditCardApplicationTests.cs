using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Xunit;

namespace CreditCards.UI.Tests
{
    public class CreditCardApplicationTests : IDisposable
    {
        private readonly IWebDriver _driver;

        public CreditCardApplicationTests()
        {
            _driver = new ChromeDriver();
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [Fact]
        public void ShouldLoadApplicationPage_SmokeTest()
        {
            _driver.Navigate().GoToUrl("http://localhost:64559/apply");

            Assert.Equal("Credit Card Application - CreditCards", _driver.Title);
        }

        [Fact]
        public void ShouldValidateApplicationDetails()
        {
            _driver.Navigate().GoToUrl("http://localhost:64559/apply");

            _driver.FindElement(By.Name("LastName")).SendKeys("Smith");
            _driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys("012345-A");
            _driver.FindElement(By.Id("Age")).SendKeys("18");
            _driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys("100000");
            _driver.FindElement(By.Id("submitApplication")).Click();

            Assert.Equal("Credit Card Application - CreditCards", _driver.Title);

            var firstErrorMessage = _driver.FindElement(By.CssSelector(".validation-summary-errors ul > li"));

            Assert.Equal("Please provide a first name", firstErrorMessage.Text);
        }
    }
}
