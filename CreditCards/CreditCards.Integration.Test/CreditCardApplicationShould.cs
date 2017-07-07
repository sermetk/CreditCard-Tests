using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace CreditCards.Integration.Test
{
    public class CreditCardApplicationShould
    {
        private const string _antiForgeryFieldName = "__AFTField";
        private const string _antiForgeryCookieName = "AFTCOokie";

        [Fact]
        public async Task RenderApplicationFormAsync()
        {
            // https://github.com/aspnet/Hosting/issues/959#issuecomment-286351703
            // Fix problem with 500 errors when running the test server
            var builder = new WebHostBuilder()
                .UseContentRoot(@"C:\Users\MIhsan\Documents\GitHub\CreditCard-Tests\CreditCards\CreditCards")
                .ConfigureAppConfiguration((hostingContext, config) =>  
                {
                    var env = hostingContext.HostingEnvironment;
                    ((ConfigurationBuilder)config)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .UseEnvironment("Development")
                .UseStartup<Startup>();
            //.UseApplicationInsights()

            var server = new TestServer(builder);

            var client = server.CreateClient();

            var response = await client.GetAsync("/apply");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("New Credit Card Application", responseString);
        }

        [Fact]
        public async Task NotAcceptPostedApplicationDetailsWithMissingFrequentFlyerNumber()
        {
            // https://github.com/aspnet/Hosting/issues/959#issuecomment-286351703
            // Fix problem with 500 errors when running the test server
            var builder = new WebHostBuilder()
                .UseContentRoot(@"C:\Users\MIhsan\Documents\GitHub\CreditCard-Tests\CreditCards\CreditCards")
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    ((ConfigurationBuilder)config)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .UseEnvironment("Development")
                .UseStartup<Startup>()
                .ConfigureServices(x => 
                {
                    x.AddAntiforgery(t => 
                    {
                        t.CookieName = _antiForgeryCookieName;
                        t.FormFieldName = _antiForgeryFieldName;
                    });
                });
            //.UseApplicationInsights()

            var server = new TestServer(builder);
            var client = server.CreateClient();

            var initialResponse = await client.GetAsync("/Apply");

            var antiForgeryCookieValue = ExtractAntiForgeryCookieValueFrom(initialResponse);
            var antiForgeryToken = ExtractAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Apply");

            postRequest.Headers.Add("Cookie", new CookieHeaderValue(_antiForgeryCookieName, antiForgeryCookieValue).ToString());

            var formData = new Dictionary<string, string>
            {
                {_antiForgeryFieldName, antiForgeryToken},
                {"FirstName", "Sarah"},
                {"LastName", "Smith"},
                {"Age", "18"},
                {"GrossAnnualIncome", "100000"}
            };

            postRequest.Content = new FormUrlEncodedContent(formData);

            var postReponse = await client.SendAsync(postRequest);

            postReponse.EnsureSuccessStatusCode();

            var responseString = await postReponse.Content.ReadAsStringAsync();

            Assert.Contains("Please provide a frequent flyer number", responseString);
        }

        private static string ExtractAntiForgeryCookieValueFrom(HttpResponseMessage response)
        {
            var antiForgeryCookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault(x => x.Contains(_antiForgeryCookieName));

            if (antiForgeryCookie is null)
                throw new ArgumentException($"Cookie '{_antiForgeryCookieName}' not found in HTTP response", nameof(response));

            var antiForgeryCookieValue = SetCookieHeaderValue.Parse(antiForgeryCookie).Value;

            return antiForgeryCookieValue;
        }

        private string ExtractAntiForgeryToken(string htmlBody)
        {
            var requestVerificationTokenMatch = Regex.Match(htmlBody, $@"\<input name=""{_antiForgeryFieldName}"" type=""hidden"" value=""([^""]+)"" \/\>");

            if (requestVerificationTokenMatch.Success)
                return requestVerificationTokenMatch.Groups[1].Captures[0].Value;

            throw new ArgumentException($"Anti forgery token '{_antiForgeryFieldName}' not found in HTML", nameof(htmlBody));
        }
    }
}
