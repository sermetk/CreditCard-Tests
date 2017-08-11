using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;

namespace CreditCards.UI.Tests.PageObjectModels
{
    public class ApplicationPage
    {
        public IWebDriver Driver { get; }

        private const string _pagePath = "apply";

        [FindsBy(How = How.Name, Using = "FirstName")]
        private IWebElement _firstName;

        [FindsBy(How = How.Name, Using = "LastName")]
        private IWebElement _lastName;

        [FindsBy(How = How.Id, Using = "FrequentFlyerNumber")]
        private IWebElement _frequentFlyerNumber;

        [FindsBy(How = How.Id, Using = "Age")]
        private IWebElement _age;

        [FindsBy(How = How.Id, Using = "GrossAnnualIncome")]
        private IWebElement _grossAnnualIncome;

        [FindsBy(How = How.Id, Using = "submitApplication")]
        private IWebElement _applyButton;

        [FindsBy(How = How.CssSelector, Using = ".validation-summary-errors ul > li")]
        private IWebElement _firstError;

        public string FirstErrorMessage => _firstError.Text;

        public ApplicationPage(IWebDriver driver)
        {
            Driver = driver;
            PageFactory.InitElements(driver, this);
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
