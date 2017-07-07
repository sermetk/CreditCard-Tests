using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CreditCards.Integration.Test
{
    public class CreditCardApplicationShould
    {
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
                .UseStartup<Startup>();
            //.UseApplicationInsights()

            var server = new TestServer(builder);

            var client = server.CreateClient();

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Apply");

            var formData = new Dictionary<string, string>
            {
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
    }
}
