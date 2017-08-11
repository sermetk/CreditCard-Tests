using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace CreditCards.UI.Tests.PageObjectModels
{
    public class ApplicationCompletePage
    {
        public IWebDriver Driver { get; }

        [FindsBy(How = How.Id, Using = "fullName")]
        private IWebElement _name;

        [FindsBy(How = How.Id, Using = "decision")]
        private IWebElement _decision;

        public string FullName => _name.Text;

        public string ApplicationDecision => _decision.Text;

        public ApplicationCompletePage(IWebDriver driver)
        {
            Driver = driver;
            PageFactory.InitElements(driver, this);
        }
    }
}
