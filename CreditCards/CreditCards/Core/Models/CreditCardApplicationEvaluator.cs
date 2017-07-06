namespace CreditCards.Core.Models
{
    public class CreditCardApplicationEvaluator
    {
        private const int _autoReferralMaxAge = 20;
        private const int _highIncomeThreshhold = 100_000;
        private const int _lowIncomeThreshhold = 20_000;

        public CreditCardApplicationDecision Evaluate(CreditCardApplication application)
        {
            if (application.GrossAnnualIncome >= _highIncomeThreshhold)
                return CreditCardApplicationDecision.AutoAccepted;

            if (application.Age <= _autoReferralMaxAge)
                return CreditCardApplicationDecision.ReferredToHuman;

            if (application.GrossAnnualIncome < _lowIncomeThreshhold)
                return CreditCardApplicationDecision.AutoDeclined;

            return CreditCardApplicationDecision.ReferredToHuman;
        }
    }
}
