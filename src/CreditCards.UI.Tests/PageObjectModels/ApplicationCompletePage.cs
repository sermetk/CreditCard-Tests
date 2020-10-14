using OpenQA.Selenium;

namespace CreditCards.UI.Tests.PageObjectModels
{
    public class ApplicationCompletePage
    {
        public IWebDriver Driver { get; }

        public IWebElement _name => Driver.FindElement(By.Id("fullName"));
        public IWebElement _decision => Driver.FindElement(By.Id("decision"));

        public string FullName => _name.Text;

        public string ApplicationDecision => _decision.Text;

        public ApplicationCompletePage(IWebDriver driver)
        {
            Driver = driver;
        }
    }
}
