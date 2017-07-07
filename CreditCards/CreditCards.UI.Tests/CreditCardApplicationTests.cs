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
    }
}
