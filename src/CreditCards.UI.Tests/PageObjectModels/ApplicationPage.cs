using OpenQA.Selenium;
using System;

namespace CreditCards.UI.Tests.PageObjectModels
{
    public class ApplicationPage
    {
        public IWebDriver Driver { get; }

        private const string _pagePath = "apply";

        public IWebElement _firstName => Driver.FindElement(By.Name("FirstName"));

        public IWebElement _lastName => Driver.FindElement(By.Name("LastName"));

        public IWebElement _frequentFlyerNumber => Driver.FindElement(By.Id("FrequentFlyerNumber"));

        public IWebElement _age => Driver.FindElement(By.Id("Age"));

        public IWebElement _grossAnnualIncome => Driver.FindElement(By.Id("GrossAnnualIncome"));

        public IWebElement _applyButton => Driver.FindElement(By.Id("submitApplication"));

        public IWebElement _firstError => Driver.FindElement(By.CssSelector(".validation-summary-errors ul > li"));

        public string FirstErrorMessage => _firstError.Text;

        public ApplicationPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public void NavigateTo()
        {
            var root = new Uri(Driver.Url).GetLeftPart(UriPartial.Authority);

            var url = $"{root}/{_pagePath}";

            Driver.Navigate().GoToUrl(url);
        }

        public void EnterName(string firstName, string lastName)
        {
            _firstName.SendKeys(firstName);
            _lastName.SendKeys(lastName);
        }

        public void EnterFrequentFlyerNumber(string frequentFlyerNumber)
        {
            _frequentFlyerNumber.SendKeys(frequentFlyerNumber);
        }

        public void EnterAge(string age)
        {
            _age.SendKeys(age);
        }

        public void EnterGrossAnnualIncome(string income)
        {
            _grossAnnualIncome.SendKeys(income);
        }

        public ApplicationCompletePage SubmitApplication()
        {
            _applyButton.Click();
            return new ApplicationCompletePage(Driver);
        }
    }
}
