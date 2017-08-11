using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CreditCards.Integration.Test
{
    public class CreditCardApplicationShould : IClassFixture<TestServerFixture>
    {
        private TestServerFixture _fixture;

        public CreditCardApplicationShould(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task RenderApplicationFormAsync()
        {
            var response = await _fixture.Client.GetAsync("/apply");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("New Credit Card Application", responseString);
        }

        [Fact]
        public async Task NotAcceptPostedApplicationDetailsWithMissingFrequentFlyerNumber()
        {
            var initialResponse = await _fixture.Client.GetAsync("/Apply");
            var antiForgeryValues = await _fixture.ExtractAntiForgeryValues(initialResponse);

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Apply");

            postRequest.Headers.Add("Cookie", new CookieHeaderValue(TestServerFixture.AntiForgeryCookieName, antiForgeryValues.cookieValue).ToString());

            var formData = new Dictionary<string, string>
            {
                {TestServerFixture.AntiForgeryFieldName, antiForgeryValues.fieldValue},
                {"FirstName", "Sarah"},
                {"LastName", "Smith"},
                {"Age", "18"},
                {"GrossAnnualIncome", "100000"}
            };

            postRequest.Content = new FormUrlEncodedContent(formData);

            var postReponse = await _fixture.Client.SendAsync(postRequest);

            postReponse.EnsureSuccessStatusCode();

            var responseString = await postReponse.Content.ReadAsStringAsync();

            Assert.Contains("Please provide a frequent flyer number", responseString);
        }
    }
}
